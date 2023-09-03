using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter :INoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings.SimpleNoiseSettings settings;

    public NoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseV = 0;
        float frecuency = settings.baseRoughtness;
        float amplitude = 1;
        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = noise.Evaluate(point * frecuency + settings.center);
            noiseV += (v + 1) / .5f * amplitude;
            frecuency *= settings.roughtness;
            amplitude *= settings.persistance;
        }
        noiseV = noiseV - settings.minValue;
        return noiseV*settings.strength;
    }
}
