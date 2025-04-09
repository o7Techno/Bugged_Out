using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotChecker
{
    public QuickSlot quickSlotSystem;
    Canvas canvas = GameObject.FindAnyObjectByType<Canvas>();
    public void CheckForEmptySlot(ref Dictionary<GameObject, SelectionManager.Pair> slotList, string itemToAdd, out bool inventoryFull)
    {
        inventoryFull = false;
        foreach (var slotData in slotList)
        {
            if (slotData.Value.item == itemToAdd  && slotData.Value.quantity < 52)
            {
                AddItemCounter(slotData.Key);
                AddItemToDictionary(ref slotList, slotData.Key, itemToAdd);
                return;
            }
        }
        foreach (var slotData in slotList)
        {
            if (slotData.Value.quantity == 0)
            {
                AddItem(itemToAdd, slotData.Key);
                AddItemToDictionary(ref slotList, slotData.Key, itemToAdd);
                return;
            }
        }
        inventoryFull = true;
    }

    static void AddItemCounter(GameObject slot)
    {
        GameObject item = slot.transform.GetChild(0).gameObject;
        TextMeshProUGUI itemCounter = item.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        int amount;
        int.TryParse(itemCounter.text, out amount);
        amount = amount == 0 ? 2 : amount + 1;
        itemCounter.text = amount.ToString();
    }

    public void RemoveItemsFromInventory(ref Dictionary<GameObject, SelectionManager.Pair> slotList, string item, int amount)
    {
        int amountToRemove = 0;
        foreach (var slotData in slotList)
        {
            if (slotData.Value.item == item && slotData.Value.quantity > 0)
            {
                if (amount <= 0)
                {
                    return;
                }
                if (amount >= slotData.Value.quantity)
                {
                    amount -= slotData.Value.quantity;
                    amountToRemove = slotData.Value.quantity;
                    RemoveItem(slotData.Key);
                }
                else
                {
                    amountToRemove = amount;
                    amount = 0;
                    RemoveItemCounter(slotData.Key, amountToRemove);
                }
                RemoveItemFromDictionary(ref slotList, slotData.Key, amountToRemove);
            }
        }
    }

    public static int CheckForItems(ref Dictionary<GameObject, SelectionManager.Pair> slotList, string item)
    {
        int itemAmount = 0;
        foreach (var slotData in slotList)
        {
            if (slotData.Value.item == item && slotData.Value.quantity > 0)
            {
                itemAmount += slotData.Value.quantity;
            }
        }
        return itemAmount;
    }

    static void RemoveItemCounter(GameObject slotToRemoveFrom, int amountToRemove)
    {
        GameObject item = slotToRemoveFrom.transform.GetChild(0).gameObject;
        TextMeshProUGUI itemCounter = item.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        int amount;
        int.TryParse(itemCounter.text, out amount);
        string text = ((amount - amountToRemove) == 1) ? "" : (amount - amountToRemove).ToString();
        itemCounter.text = text;
    }

    void RemoveItem(GameObject slotToRemoveFrom)
    {
        if (slotToRemoveFrom.name.Contains("Quick") && String.Equals(quickSlotSystem.selectedSlot.ToString(), slotToRemoveFrom.name.Split(" ")[1]))
        {
            quickSlotSystem.DestroyItem();
        }
        GameObject.Destroy(slotToRemoveFrom.transform.GetChild(0).gameObject);
    }

    static void RemoveItemFromDictionary(ref Dictionary<GameObject, SelectionManager.Pair> slotList, GameObject slotToRemoveFrom, int amount)
    {
        slotList[slotToRemoveFrom].quantity -= amount;
        if (slotList[slotToRemoveFrom].quantity <= 0)
        {
            slotList[slotToRemoveFrom].quantity = 0;
            slotList[slotToRemoveFrom].item = "";
        }
    }

    public void AddItem(string itemToAdd, GameObject slotToAddTo)
    {
        if (slotToAddTo.name.Contains("Quick") && String.Equals(quickSlotSystem.selectedSlot.ToString(), slotToAddTo.name.Split(" ")[1]))
        {
            quickSlotSystem.CreateItem(quickSlotSystem.selectedSlot);
        }
        GameObject newItem = GameObject.Instantiate(Resources.Load<GameObject>(itemToAdd), slotToAddTo.transform.position, slotToAddTo.transform.rotation);
        newItem.transform.SetParent(slotToAddTo.transform);
        newItem.GetComponent<DragNDrop>().canvas = canvas;
    }

    public static void AddItemToDictionary(ref Dictionary<GameObject, SelectionManager.Pair> slotList, GameObject slotToAddTo, string itemToAdd)
    {
        if (slotList[slotToAddTo].quantity > 0)
        {
            slotList[slotToAddTo].quantity += 1;
        }
        else if (slotList[slotToAddTo].quantity == 0)
        {
            slotList[slotToAddTo].quantity = 1;
            slotList[slotToAddTo].item = itemToAdd;
        }
    }
}
