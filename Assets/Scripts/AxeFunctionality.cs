using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeFunctionality : MonoBehaviour
{
    Animator animator;
    InventorySystem inventorySystem;
    SelectionManager selectionManager;
    bool isHitting;

    private void Start()
    {
        isHitting = false;
        selectionManager = FindFirstObjectByType<SelectionManager>();
        animator = GetComponent<Animator>();
        inventorySystem = GameObject.FindGameObjectWithTag("InventorySystem").GetComponent<InventorySystem>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !inventorySystem.isOpen)
        {
            animator.SetTrigger("hit");
            isHitting = true;
        }
        else if ((Input.GetMouseButtonUp(0) || inventorySystem.isOpen) && isHitting)
        {
            isHitting = false;
            animator.SetTrigger("stopHitting");
        }
    }

    public void HitTree()
    {
        GameObject selectedTree = selectionManager.selectedTree;
        if (selectedTree)
        {
            selectedTree.GetComponent<ChoppableTree>().GetHit(1);
        }
    }

}
