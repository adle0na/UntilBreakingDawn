using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Light Preset", menuName = "Scripatbles/Lighting Preset", order = 1)]

public class LightPreset : ScriptableObject
{
    public Gradient _AmbientColor;
    public Gradient _DirectionalColor;
    public Gradient _FogColor;
}
