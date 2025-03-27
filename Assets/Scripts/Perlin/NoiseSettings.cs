using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    [Range(0f, 1f)]
    public float frequency = 1f;

    [Range(0f, 10f)]
    public float amplitude = 10f;
}
