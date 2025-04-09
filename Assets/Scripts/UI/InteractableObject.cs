using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;

    public bool pickable;

    public bool choppable;

    public bool punchable;

    public string GetItemName()
    {
        return ItemName;
    }

}