using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragNDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    QuickSlot quickSlotSystem;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    TextMeshProUGUI TMPDragged;
    TextMeshProUGUI TMPLeft;
    int amountBeforeDrag;
    int amountLeft;
    GameObject newItem;

    public static GameObject itemBeingDragged;
    public Canvas canvas;

    Vector3 startPosition;
    Transform startParent;
    

    private void Awake()
    {
        quickSlotSystem = GameObject.FindObjectOfType<QuickSlot>();
        TMPDragged = transform.GetChild(0).GetComponent<TextMeshProUGUI>(); 
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = FindObjectsByType<MainUI>(FindObjectsSortMode.None).ToList<MainUI>()[0].gameObject.GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
            return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (transform.parent.name.Contains("QuickSlot") && transform.parent.name.Contains(quickSlotSystem.selectedSlot.ToString()))
            {
                quickSlotSystem.DestroyItem();
            }
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
            startPosition = transform.position;
            startParent = transform.parent;
            transform.SetParent(transform.root, true);
            itemBeingDragged = gameObject;
            amountLeft = 0;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
            startPosition = transform.position;
            startParent = transform.parent;
            itemBeingDragged = gameObject;
            int amountDragged;
            int.TryParse(TMPDragged.text, out amountBeforeDrag);
            amountBeforeDrag = amountBeforeDrag == 0 ? 1 : amountBeforeDrag;
            amountDragged = (amountBeforeDrag % 2 == 0) ? amountBeforeDrag / 2 : amountBeforeDrag / 2 + 1;
            amountLeft = amountBeforeDrag - amountDragged;
            TMPDragged.text = amountDragged > 1 ? amountDragged.ToString() : "";
            if (transform.parent.name.Contains("QuickSlot") && transform.parent.name.Contains(quickSlotSystem.selectedSlot.ToString()) && amountLeft == 0)
            {
                quickSlotSystem.DestroyItem();
            }
            if (amountLeft > 0)
            {
                newItem = GameObject.Instantiate(Resources.Load<GameObject>(itemBeingDragged.name.Replace("(Clone)", "")), startParent.position, startParent.rotation);
                newItem.transform.SetParent(startParent);
                TMPLeft = newItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TMPLeft.text = amountLeft > 1 ? amountLeft.ToString() : "";

            }
            transform.SetParent(transform.root, true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent.name.Contains("QuickSlot") && transform.parent.name.Contains(quickSlotSystem.selectedSlot.ToString()))
        {
            quickSlotSystem.CreateItem(quickSlotSystem.selectedSlot);
        }
        itemBeingDragged = null;

        if (transform.parent == transform.root)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
            if (amountLeft > 0)
            {
                TMPDragged.text = amountBeforeDrag.ToString();
                GameObject.Destroy(newItem);
            }
            if (startParent.name.Contains("QuickSlot") && startParent.name.Contains(quickSlotSystem.selectedSlot.ToString()) && !quickSlotSystem.selectedItem)
            {
                quickSlotSystem.CreateItem(quickSlotSystem.selectedSlot);
            }
        }
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
}
