using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    [SerializeField] private float SpeedMultiplier;
    [SerializeField, Range(0, 24)] public float TimeOfDay;
    [SerializeField] private ReflectionProbe repro;

    [SerializeField] private WorldStateTracker Tracker;

    public void OnEnable()
    {
        Tracker = GameObject.Find("WorldStateTracker").GetComponent<WorldStateTracker>();

        TimeOfDay = Tracker.TimeOfDay;
    }

    public void Update()
    {
        if (Preset == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime * SpeedMultiplier;

            if (TimeOfDay >= 24)
            {
                Tracker.Day++;
                Tracker.UpdateTracker();
            }

            TimeOfDay %= 24;

            UpdateLighting(TimeOfDay / 24f);
            //Debug.Log("Time of Day " + TimeOfDay);

            try
            {
                if (TimeOfDay > 6 && TimeOfDay <= 18) 
                {
                    repro.intensity = 1;
                }
                else
                {
                    repro.intensity = 0;
                }

            }
            catch (System.Exception)
            {
                //Debug.Log("No Reflection Probe");   
            }


        }
        else
        {
            try
            {
                if (TimeOfDay > 6 && TimeOfDay <= 18)
                {
                    repro.intensity = 1;
                }
                else
                {
                    repro.intensity = 0;
                }

            }
            catch (System.Exception)
            {
                //Debug.Log("No Reflection Probe");   
            }

            UpdateLighting(TimeOfDay / 24f);
            //Debug.Log("Time of Day " + TimeOfDay);
        }
    }

    void UpdateLighting(float TimePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(TimePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(TimePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(TimePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((TimePercent * 360f) - 90f, 170, 0));
        }
    }
    public void OnValidate()
    {
        if (DirectionalLight != null)
        {
            return;
        }
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] Lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in Lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
