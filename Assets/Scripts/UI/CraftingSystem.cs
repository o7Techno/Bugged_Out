using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public bool isOpen;
    public GameObject craftingUI;
    public GameObject inventoryUI;
    InventorySystem inventorySystem;
    public RecipeData knifeRecipe;
    public RecipeData axeRecipe;
    QuickSlot quickSlotSystem;
    public Dictionary<string, int> itemAmounts;

    public event Action RefreshAmounts;

    public enum Material
    {
        Vine,
        Rock,
        Branch,
        Log,
        Knife,
        Axe
    }
    private void Awake()
    {
        itemAmounts = new Dictionary<string, int>();
        quickSlotSystem = GameObject.FindObjectOfType<SelectionManager>().quickSlotSystem;
        inventorySystem = GameObject.FindGameObjectWithTag("InventorySystem").GetComponent<InventorySystem>();
    }

    private void Start()
    {

        Transform knifeObject = craftingUI.transform.Find("Knife");
        Button knifeButton = knifeObject.Find("Button").GetComponent<Button>();
        knifeButton.onClick.AddListener(delegate{ CraftItem(knifeRecipe); });

        Transform axeObject = craftingUI.transform.Find("Axe");
        Button axeButton = axeObject.Find("Button").GetComponent<Button>();
        axeButton.onClick.AddListener(delegate { CraftItem(axeRecipe); });

        isOpen = false;
        craftingUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isOpen && inventorySystem.isOpen)
        {
            Refresh();
            craftingUI.SetActive(true);
            inventoryUI.transform.localPosition = new Vector3(850, 0, 0);
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isOpen)
        {
            craftingUI.SetActive(false);
            inventoryUI.transform.localPosition = new Vector3(0, 0, 0);
            isOpen = false;
        }

    }

    void Refresh()
    {
        itemAmounts.Clear();
        foreach (var item in inventorySystem.slotList.Values)
        {
            if (item.item != "")
            {
                if (itemAmounts.ContainsKey(item.item))
                {
                    itemAmounts[item.item] += item.quantity;
                }
                else
                {
                    itemAmounts.Add(item.item, item.quantity);
                }
            }
        }
        RefreshAmounts?.Invoke();
    }

    void CraftItem(RecipeData recipe)
    {
        StartCoroutine(CraftingCoroutine(recipe));
    }
    
    IEnumerator CraftingCoroutine(RecipeData recipe)
    {
        foreach (Pair<string, int> item in recipe.requirements)
        {
            inventorySystem.RemoveItems(item.first, item.second);
        }
        yield return 0;
        inventorySystem.AddItem(recipe.item);
        Refresh();
    }
}
