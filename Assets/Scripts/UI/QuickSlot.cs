using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    public GameObject numberHolder;
    public GameObject quickSlots;
    public Transform itemHolder;
    public int selectedSlot;
    public GameObject selectedItem;
    GameObject instantiatedItem;
    TextMeshProUGUI previousText;
    InventorySystem inventorySystem;

    public Transform parent;

    private void Start()
    {
        previousText = numberHolder.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        inventorySystem = GameObject.FindGameObjectWithTag("InventorySystem").GetComponent<InventorySystem>();
        selectedSlot = 1;
        selectedItem = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !inventorySystem.isOpen && selectedSlot != 1)
        {
            SelectSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !inventorySystem.isOpen && selectedSlot != 2)
        {
            SelectSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !inventorySystem.isOpen && selectedSlot != 3)
        {
            SelectSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && !inventorySystem.isOpen && selectedSlot != 4)
        {
            SelectSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && !inventorySystem.isOpen && selectedSlot != 5)
        {
            SelectSlot(5);
        }
    }

    public void SelectSlot(int number)
    {
        previousText.color = Color.gray;
        selectedSlot = number;
        DestroyItem();
        previousText = numberHolder.transform.GetChild(number - 1).GetComponent<TextMeshProUGUI>();
        previousText.color = Color.white;
        CreateItem(number);

    }

    public void DestroyItem()
    {
        if (instantiatedItem)
        {
            StartCoroutine(nameof(DestroyCoroutine));
        }
    }

    IEnumerator DestroyCoroutine()
    {
        yield return null;
        GameObject.Destroy(instantiatedItem.gameObject);
        selectedItem = null;
    }

    IEnumerator CreateCoroutine(int number)
    {
        yield return null;
        selectedItem = GetSelectedItem(number);
        if (selectedItem)
        {
            instantiatedItem = GameObject.Instantiate(Resources.Load<GameObject>(selectedItem.name.Replace("(Clone)", "") + "_Model"), new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
            instantiatedItem.transform.SetParent(parent);
            instantiatedItem.transform.localPosition = Vector3.zero;
            instantiatedItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (itemHolder.childCount > 1)
        {
            for (int i = 0; i < itemHolder.childCount - 1; i++)
            {
                GameObject.Destroy(itemHolder.GetChild(i).gameObject);
            }
        }

    }

    public void CreateItem(int number)
    {
        StartCoroutine(CreateCoroutine(number));
    }

    public GameObject GetSelectedItem(int number)
    {
        if (quickSlots.transform.GetChild(number - 1).transform.childCount > 0)
        {
            return quickSlots.transform.GetChild(number - 1).transform.GetChild(0).gameObject;
        }
        return null;
    }
}
