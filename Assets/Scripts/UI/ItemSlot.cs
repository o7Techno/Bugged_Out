using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    QuickSlot quickSlotSystem;
    void Awake()
    {
        quickSlotSystem = GameObject.FindObjectOfType<QuickSlot>();
    }
    public GameObject item
    {
        get
        {
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public int itemAmount
    {
        get
        {
            if (!item)
            {
                return 0;
            }
            int amount;
            int.TryParse(transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text, out amount);
            amount = amount == 0 ? 1 : amount;
            return amount;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            DragNDrop.itemBeingDragged.transform.SetParent(transform);
            DragNDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);
        }
        else if (item.ToString() == DragNDrop.itemBeingDragged.ToString() && itemAmount < 52)
        {
            int amountToAdd;
            int.TryParse(DragNDrop.itemBeingDragged.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text, out amountToAdd);
            amountToAdd = amountToAdd == 0 ? 1 : amountToAdd;
            GameObject.Destroy(DragNDrop.itemBeingDragged);
            transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (itemAmount + amountToAdd).ToString();
        }
    }
}
