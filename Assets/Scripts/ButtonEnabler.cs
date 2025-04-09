using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEnabler : MonoBehaviour
{
    GameObject button;
    public RecipeData recipe;
    CraftingSystem craftingSystem;
    private void Awake()
    {
        button = GetComponentInChildren<Button>().gameObject;
        craftingSystem = FindObjectOfType<CraftingSystem>();
    }

    private void Start()
    {
        craftingSystem.RefreshAmounts += RefreshButton;
        button.SetActive(false);
    }

    public void RefreshButton()
    {
        bool isSufficient = true;
        foreach (var item in recipe.requirements)
        {
            int amount;
            craftingSystem.itemAmounts.TryGetValue(item.first, out amount);
            if (amount < item.second)
            {
                isSufficient = false;
            }
        }

        if (isSufficient)
        {
            button.SetActive(true);
        }
        else
        {
            button.SetActive(false);
        }
    }
}
