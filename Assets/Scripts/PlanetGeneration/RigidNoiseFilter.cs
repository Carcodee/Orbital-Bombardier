using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter :INoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings.RigidNoiseSettings settings;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseV = 0;
        float frecuency = settings.baseRoughtness;
        float amplitude = 1;
        float weight = 1;
        for (int i = 0; i < settings.numLayers; i++)
        {
            float v =1-Mathf.Abs( noise.Evaluate(point * frecuency + settings.center));
            v *= v;
            v *= weight;
            weight =Mathf.Clamp01 (v* settings.weightMultplier);
            noiseV += v  * amplitude;
            
            frecuency *= settings.roughtness;
            amplitude *= settings.persistance;
        }
        noiseV = noiseV - settings.minValue;
        return noiseV * settings.strength;
    }
}
