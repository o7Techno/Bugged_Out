using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeFunctionality : MonoBehaviour
{
    Animator animator;
    InventorySystem inventorySystem;
    SelectionManager selectionManager;
    public float knifeCooldown = 0.6f;
    public float knifeRange = 2f;
    public int knifeDamage = 2;
    bool isOnCooldown;

    private void Start()
    {
        isOnCooldown = false;
        selectionManager = FindFirstObjectByType<SelectionManager>();
        animator = GetComponent<Animator>();
        inventorySystem = GameObject.FindGameObjectWithTag("InventorySystem").GetComponent<InventorySystem>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !inventorySystem.isOpen && !isOnCooldown)
        {
            animator.SetTrigger("hit");
            isOnCooldown = true;
            Invoke(nameof(UpdateCooldown), knifeCooldown);
        }
    }

    void UpdateCooldown()
    {
        isOnCooldown = false;
    }


    public void Hit()
    {
        GameObject selectedMob = selectionManager.selectedMob;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (selectedMob && Physics.Raycast(ray, out hit, knifeRange) && hit.transform.gameObject == selectedMob)
        {
            selectedMob.GetComponent<PunchableObject>().GetHit(knifeDamage);
        }
    }
}
