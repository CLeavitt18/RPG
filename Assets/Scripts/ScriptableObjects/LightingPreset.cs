using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Lighting Preset", menuName = "Lighting Preset")]

public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}
