using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu()]
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColour;
    public Gradient DirectionalColour;
    public Gradient FogColour;

}
