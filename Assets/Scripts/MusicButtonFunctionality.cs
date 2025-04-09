using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButtonFunctionality : MonoBehaviour
{
    public AudioSource source;
    public void OnClick()
    {
        source.mute = !source.mute;
    }
}
