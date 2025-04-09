using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSliderFunctionality : MonoBehaviour
{
    public AudioSource source;
    public Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void onChangeVolume()
    {
        source.volume = slider.value;
    }
    
}
