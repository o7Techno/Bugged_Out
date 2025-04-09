using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodFunctionality : MonoBehaviour
{
    public float foodValue;
    public string foodName;
    Animator animator;
    InventorySystem inventorySystem;
    SelectionManager selectionManager;
    PlayerHunger playerHunger;
    bool isEating;

    private void Start()
    {
        isEating = false;
        animator = GetComponent<Animator>();
        inventorySystem = GameObject.FindGameObjectWithTag("InventorySystem").GetComponent<InventorySystem>();
        playerHunger = GameObject.FindAnyObjectByType<PlayerHunger>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !inventorySystem.isOpen)
        {
            animator.SetTrigger("eat");
            isEating = true;
        }
        else if ((Input.GetMouseButtonUp(1) || inventorySystem.isOpen) && isEating)
        {
            isEating = false;
            animator.SetTrigger("stopEating");
        }
    }

    void FinishEating()
    {
        if (playerHunger.Hunger + foodValue >= 100f)
        {
            playerHunger.Hunger = 100f;
        }
        else
        {
            playerHunger.Hunger += foodValue;
        }
        inventorySystem.RemoveItems(foodName, 1);
    }
}
