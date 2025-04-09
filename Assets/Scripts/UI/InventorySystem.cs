using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public bool isOpen;
    public GameObject inventoryUI;
    public ItemSlotChecker itemSlotChecker;
    public Transform inventory;
    public Transform quickSlots;
    bool inventoryIsFull;
    CraftingSystem craftingSystem;
    public Dictionary<GameObject, SelectionManager.Pair> slotList = new Dictionary<GameObject, SelectionManager.Pair>();

    private void Awake()
    {
        craftingSystem = GameObject.FindGameObjectWithTag("CraftingSystem").GetComponent<CraftingSystem>();
        SelectionManager selectionManager = GameObject.FindFirstObjectByType<SelectionManager>();
        itemSlotChecker = new ItemSlotChecker();
        itemSlotChecker.quickSlotSystem = selectionManager.quickSlotSystem;
        inventoryUI.SetActive(false);
        isOpen = false;
        
    }
    private void Start()
    {
        foreach (Transform slot in inventory)
        {
            slotList.Add(slot.gameObject, new SelectionManager.Pair("", 0));
        }
        inventoryIsFull = false;
    }
    public void AddItem(string item)
    {
        RefreshSlotList();
        itemSlotChecker.CheckForEmptySlot(ref slotList, item, out inventoryIsFull);
    }

    public void RemoveItems(string item, int amount)
    {
        RefreshSlotList();
        itemSlotChecker.RemoveItemsFromInventory(ref slotList, item, amount);
        RefreshSlotList();
    }

    public int CheckForItems(string item, int amount)
    {
        RefreshSlotList();
        return ItemSlotChecker.CheckForItems(ref slotList, item);
    }

    public void RefreshSlotList()
    {
        slotList.Clear();
        foreach (Transform quickSlot in quickSlots)
        {
            if (quickSlot.gameObject.GetComponent<ItemSlot>().itemAmount != 0)
            {
                slotList.Add(quickSlot.gameObject,  new SelectionManager.Pair(quickSlot.GetComponent<ItemSlot>().item.name.Replace("(Clone)", ""), quickSlot.GetComponent<ItemSlot>().itemAmount));
            }
            else
            {
                slotList.Add(quickSlot.gameObject, new SelectionManager.Pair("", 0));
            }
        }
        foreach (Transform slot in inventory)
        {
            if (slot.gameObject.GetComponent<ItemSlot>().itemAmount != 0)
            {
                slotList.Add(slot.gameObject, new SelectionManager.Pair(slot.GetComponent<ItemSlot>().item.name.Replace("(Clone)", ""), slot.GetComponent<ItemSlot>().itemAmount));
            }
            else
            {
                slotList.Add(slot.gameObject, new SelectionManager.Pair("", 0));

            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            inventory.localPosition = new Vector3(0, 0, 0);
            inventoryUI.SetActive(true);
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && isOpen)
        {
            inventoryUI.SetActive(false);
            craftingSystem.craftingUI.SetActive(false);
            isOpen = false;
            craftingSystem.isOpen = false;
        }
    }
}
