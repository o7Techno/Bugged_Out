using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCHealthBar : MonoBehaviour
{
    public Slider slider;
    public int Health { get; private set; }


    //IEnumerator HealthCoroutine(float start, float end)
    //{
    //    float cur = start;
    //    if (end > start)
    //    {
    //        while (start < end)
    //        {
    //            cur += 0.1f;
    //            cur = end < cur ? end : cur;
    //            slider.value = cur;
    //            yield return null;
    //        }
    //    }
    //    else
    //    {
    //        while(start > end)
    //        {
    //            cur -= 0.1f;
    //            cur = end > cur ? end : cur;
    //            slider.value = cur;
    //            yield return null;
    //        }
    //    }
    //}

    public void SetHealth(int health)
    {
        //float value = slider.value;
        slider.value = health;

        //StartCoroutine(HealthCoroutine(value, health));

    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
    }
}
