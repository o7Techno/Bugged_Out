using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchableObject : MonoBehaviour
{
    public int objectMaxHealth;
    public int objectCurrentHealth;
    NPCHealthBar objectHealthBar;

    private void Awake()
    {
        objectHealthBar = GameObject.FindObjectOfType<NPCHealthBar>(true);
    }
    private void Start()
    {
        objectCurrentHealth = objectMaxHealth;
    }

    public void GetHit(int damage)
    {
        objectCurrentHealth -= damage;
        transform.parent.GetComponent<MobAI>().GetHit(damage);
        if (objectCurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameObject.Destroy(transform.parent.gameObject);
        objectHealthBar.gameObject.SetActive(false);
        transform.parent.GetComponent<MobAI>().Die();
    }
}
