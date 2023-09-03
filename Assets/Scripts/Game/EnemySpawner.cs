using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour,ISaveable
{
    public Transform player;
    public Transform enemyContainer;
    public float timeToSpawn;
    public float currentTime;
    public GameController gameController;
    public float offSet;
    public EnemyStats enemy;
    public int startAmountOfEnemys;
    private void Awake()
    {

    }
    void Start()
    {
        gameController = GetComponent<GameController>();

        if (GameManager.instance.isNewGame)
        {
            for (int i = 0; i < startAmountOfEnemys; i++)
            {
                SpawnEnemyOnPosition();
            }
        }
        else
        {
            LoadData();
        }
        
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (GameManager.instance.currentState==GameManager.GameState.onGame && currentTime>timeToSpawn)
        {
            SpawnEnemyOnPosition();
            currentTime = 0;

        }
    }
    public void SpawnEnemyOnPosition()
    {
 
        EnemyStats enemyInstance = Instantiate(enemy, Vector3.zero, Quaternion.identity,enemyContainer);
        enemyInstance.GetComponent<EnemyPatrol>().player = player;
        gameController.SetOrientation(enemyInstance.transform);
        enemyInstance.transform.position = enemyInstance.transform.up * enemy.GetComponent<EnemyPatrol>().radius;

    }
    


    public void SaveData()
    {
        SaveSystem.SaveEnemyData(enemyContainer);
        Debug.Log("save it");

    }

    public void LoadData()
    {
        for (int i = 0; i < SaveSystem.LoadEnemyData().enemyAmount; i++)
        {
            Vector3[] positions = new Vector3[SaveSystem.LoadEnemyData().enemyAmount * 3];
            Quaternion [] rotation = new Quaternion[SaveSystem.LoadEnemyData().enemyAmount * 4];
        
            EnemyStats enemyInstance = Instantiate(enemy, Vector3.zero, Quaternion.identity,enemyContainer);
            for (int j = 0; j < SaveSystem.LoadEnemyData().enemyAmount; j+=3)
            {
                positions[j].x = SaveSystem.LoadEnemyData().pos[j];
                positions[j].y = SaveSystem.LoadEnemyData().pos[j+1];
                positions[j].z = SaveSystem.LoadEnemyData().pos[j+2];
                enemyInstance.transform.position = positions[j];

            }
            for (int j = 0; j < SaveSystem.LoadEnemyData().enemyAmount; j+=4)
            {
                rotation[j].x = SaveSystem.LoadEnemyData().pos[j];
                rotation[j].y = SaveSystem.LoadEnemyData().pos[j+1];
                rotation[j].z = SaveSystem.LoadEnemyData().pos[j+2];
                rotation[j].w = SaveSystem.LoadEnemyData().pos[j+3];
                enemyInstance.transform.rotation = rotation[j];
            }
        
        
            enemyInstance.GetComponent<EnemyPatrol>().player = player;
            gameController.SetOrientation(enemyInstance.transform);
            enemyInstance.transform.position = enemyInstance.transform.up * enemy.GetComponent<EnemyPatrol>().radius;
        }
        
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
