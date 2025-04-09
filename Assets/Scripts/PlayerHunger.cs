using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    public HungerBar hungerBar;
    public float hungerTime;
    public int healthTime;
    public PlayerHealth health;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        hunger = 100;
        hungerBar.SetMaxHunger(hunger);
    }

    private void Update()
    {
        if (health.Health < 100 && hunger > 80)
        {
            health.Health += Time.deltaTime * 100 / healthTime;
        }
        Hunger -= Time.deltaTime / hungerTime * 100;
    }

    float hunger;
    public float Hunger
    {
        get { return hunger; }
        set
        {
            hunger = value;
            hungerBar.SetHunger(hunger);
        }
    }
}
