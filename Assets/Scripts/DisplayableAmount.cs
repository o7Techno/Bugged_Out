using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayableAmount : MonoBehaviour
{
    public CraftingSystem.Material material;
    public int requiredAmount;
    CraftingSystem craftingSystem;
    int amount;
    public bool sufficient;
    TextMeshProUGUI amountText;

    private void Awake()
    {
        craftingSystem = GameObject.FindAnyObjectByType<CraftingSystem>();
        amountText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        amount = 0;
        int.TryParse(amountText.text.Split(" ")[2], out requiredAmount);
        sufficient = false;
        craftingSystem.RefreshAmounts += RefreshAmounts;
        RefreshAmounts();
    }
    public void RefreshAmounts()
    {
        craftingSystem.itemAmounts.TryGetValue(material.ToString(), out amount);
        if (amount >= requiredAmount)
        {
            sufficient = true;
            amountText.color = Color.green;
        } else
        {
            sufficient = false;
            amountText.color = Color.red;
        }
        amountText.text = $"{amount} / {requiredAmount}";
    }
}
