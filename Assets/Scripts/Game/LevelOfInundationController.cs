using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOfInundationController : MonoBehaviour,ISaveable
{

    public ShapeSettings planetShape;
    public float timeToDestructionRate;
    void Start()
    {
        if (GameManager.instance.isNewGame)
        {
            planetShape.noiseLayers[planetShape.noiseLayers.Length - 1].noiseSettings.simpleNoiseSettings.minValue = 5;
        }
        else
        {
            LoadData();
        }

        timeToDestructionRate = (GameManager.instance.rateOfPlanetDestruction)*10;
    }

    void Update()
    {
        if (GameManager.instance.currentState == GameManager.GameState.onGame)
        {
            planetShape.noiseLayers[planetShape.noiseLayers.Length - 1].
                noiseSettings.simpleNoiseSettings.minValue-=timeToDestructionRate;   

        }
    }

    
    //needs to be implemented
    public void LoadData()
    {
        planetShape.noiseLayers[planetShape.noiseLayers.Length - 1].
            noiseSettings.simpleNoiseSettings.minValue = SaveSystem.LoadDataPlanet().minValue;
        GameManager.instance.supplyDropped = SaveSystem.LoadDataPlanet().time;
    }

    public void SaveData()
    {
        SaveSystem.SaveLevelOfDestruction(planetShape);
    }

    public void SetDataNewGame()
    {
        planetShape.noiseLayers[planetShape.noiseLayers.Length - 1].noiseSettings.simpleNoiseSettings.minValue = 5;
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
