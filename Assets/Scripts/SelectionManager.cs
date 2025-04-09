using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public GameObject interaction_Info_UI;
    public GameObject treeHealthBarUI;
    public GameObject mobHealthBarUI;
    Text interaction_text;
    public float maxDistance;
    public float maxReachDistance;
    public float maxChopDistance;
    public float maxHitDistance;
    public QuickSlot quickSlotSystem;
    public GameObject selectedTree;
    public GameObject selectedMob;
    bool inventoryIsFull;
    InventorySystem inventorySystem;
    TreeHealthBar treeHealthBar;
    NPCHealthBar mobHealthBar;


    private void Awake()
    {
        inventorySystem = GameObject.FindGameObjectWithTag("InventorySystem").GetComponent<InventorySystem>();
        treeHealthBar = GameObject.FindObjectOfType<TreeHealthBar>(true);
        mobHealthBar = GameObject.FindObjectOfType<NPCHealthBar>(true);
    }
    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    void Update()
    {
        if (!inventorySystem.isOpen)
        {
            Checker();
        }
    }

    //Need to optimize tree checks (reassigning) and health bar checks (same problem), maybe also selected text with the same issue. 
    void Checker()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool selectDistance = Physics.Raycast(ray, out hit, maxDistance);
        if (selectDistance)
        {
            var selectionTransform = hit.transform;
            if (selectionTransform.GetComponent<InteractableObject>())
            {
                if (selectionTransform.GetComponent<InteractableObject>().choppable && hit.distance <= maxChopDistance)
                {
                    selectedTree = selectionTransform.GetComponent<InteractableObject>().gameObject;
                    ChoppableTree choppableTree = selectedTree.GetComponent<ChoppableTree>();
                    treeHealthBar.SetMaxHealth(choppableTree.treeMaxHealth);
                    treeHealthBar.SetHealth(choppableTree.treeCurrentHealth);
                    treeHealthBarUI.SetActive(true);
                }

                if(selectionTransform.GetComponent<InteractableObject>().punchable && hit.distance <= maxHitDistance)
                {
                    selectedMob = selectionTransform.GetComponent<InteractableObject>().gameObject;
                    PunchableObject punchableMob = selectedMob.GetComponent<PunchableObject>();
                    mobHealthBar.SetMaxHealth(punchableMob.objectMaxHealth);
                    mobHealthBar.SetHealth(punchableMob.objectCurrentHealth);
                    mobHealthBarUI.SetActive(true);

                }

                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (selectionTransform.GetComponent<InteractableObject>().pickable && hit.distance <= maxReachDistance)
                    {
                        inventorySystem.AddItem(selectionTransform.GetComponent<InteractableObject>().GetItemName());
                        if (!inventoryIsFull)
                        {
                            Destroy(hit.transform.gameObject);
                        }
                    }
                }
            }
            else
            {
                interaction_Info_UI.SetActive(false);
                selectedTree = null;
                selectedMob = null;
                treeHealthBarUI.SetActive(false);
                mobHealthBarUI.SetActive(false);
            }

        }
        else
        {
            interaction_Info_UI.SetActive(false);
            selectedTree = null;
            selectedMob = null;
            treeHealthBarUI.SetActive(false);
            mobHealthBarUI.SetActive(false);
        }
    }

    public class Pair
    {
        public Pair(string item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
        public string item { get; set; }
        public int quantity { get; set; }
    }
}