using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableTree : MonoBehaviour
{
    public int treeMaxHealth;
    public int treeCurrentHealth;
    TreeHealthBar treeHealthBar;

    private void Awake()
    {
        treeHealthBar = GameObject.FindObjectOfType<TreeHealthBar>(true);
    }
    private void Start()
    {
        treeCurrentHealth = treeMaxHealth;
    }

    public void GetHit(int damage)
    {
        treeCurrentHealth -= damage;
        if (treeCurrentHealth == 0)
        {
            Vector3 position = transform.parent.position;
            Quaternion rotation = transform.parent.rotation;
            GameObject.Destroy(gameObject);
            treeHealthBar.gameObject.SetActive(false);
            GameObject.Instantiate(Resources.Load<GameObject>("ChoppedTree"), position, rotation);
        }
    }
}
