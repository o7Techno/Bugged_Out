using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeData : ScriptableObject {
    public int amountOfItems;
    public string item;
    public List<Pair<string, int>> requirements;
}
