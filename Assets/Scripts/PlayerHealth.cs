using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour 
{
    public HealthBar healthBar;
    public GameObject gameOverScreen;
    public GameObject inventory;
    public GameObject crafting;

    private void Start()
    {
        health = 100;
        healthBar.SetMaxHealth(health);
    }

    float health;
    public float Health
    {
        get { return health; } 
        set
        {
            health = value;
            healthBar.SetHealth(health);
            if (health <= 0)
            {
                inventory.SetActive(false);
                crafting.SetActive(false);
                gameOverScreen.SetActive(true);
                gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
