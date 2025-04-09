using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Slider slider;
    public float Hunger { get; private set; }

    public void SetHunger(float health)
    {
        slider.value = health;
    }

    public void SetMaxHunger(float health)
    {
        slider.maxValue = health;
    }
}
