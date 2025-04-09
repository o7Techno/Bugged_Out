using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField]Light directionalLight;
    [SerializeField]LightingPreset lightingPreset;
    TimeEvents timeEvents;

    private void Start()
    {
        timeEvents = GameObject.FindGameObjectWithTag("Time").GetComponent<TimeEvents>();
    }

    private void Update()
    {
        if (lightingPreset == null)
        {
            return;
        }
        if (Application.isPlaying && !timeEvents.pauseTime)
        {
            timeEvents.timeOfDay += Time.deltaTime/20;
            timeEvents.timeOfDay %= 24;
            UpdateLighting(timeEvents.timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(timeEvents.timeOfDay /24f);
        }
    }


    void UpdateLighting(float timePercent)
    {
        UnityEngine.RenderSettings.ambientLight = lightingPreset.AmbientColour.Evaluate(timePercent);
        UnityEngine.RenderSettings.fogColor = lightingPreset.FogColour.Evaluate(timePercent);
        if (directionalLight != null)
        {
            directionalLight.color = lightingPreset.DirectionalColour.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f)-90f, 170, 0));
        }
    }

    private void OnValidate()
    {
        if (directionalLight != null)
            return;
        if(UnityEngine.RenderSettings.sun != null)
        {
            directionalLight = UnityEngine.RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if(light.type == UnityEngine.LightType.Directional)
                {
                   directionalLight = light;
                    return;
                }
            }
        }
    }

}
