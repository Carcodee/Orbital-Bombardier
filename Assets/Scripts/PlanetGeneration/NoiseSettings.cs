using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { Simple, Ridgid }
    public FilterType filterType;

    public SimpleNoiseSettings simpleNoiseSettings;
    public RigidNoiseSettings rigidNoiseSettings;
    [System.Serializable]
    public class SimpleNoiseSettings
    {

        public float strength = 1;
        public float roughtness = 2;
        public Vector3 center;
        [Range(1, 8)]
        public int numLayers = 1;
        public float persistance = .5f;
        public float baseRoughtness = 1;
        public float minValue;
    }
    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultplier = .8f;

    }


}
