
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public  class SaveSystem 
{
    public static void SaveData(PlayerStatsController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerDataSavedBinary.txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        float[] pos = new float[3];
        
        pos[0] = player.transform.position.x;
        pos[1]=player.transform.position.y;
        pos[2]=player.transform.position.z;
        
        float[] rotation = new float[4];
        rotation[0] = player.transform.rotation.x;
        rotation[1]= player.transform.rotation.y;
        rotation[2]= player.transform.rotation.z;
        rotation[3]= player.transform.rotation.w;
        
        PlayerOnSaveData dataSaved = new PlayerOnSaveData(pos, rotation, player.playerScriptableData.health, player.playerScriptableData.suppliesOnCharge ,player.playerScriptableData.speed, 
            player.playerScriptableData.boostSpeed, player.playerScriptableData.turnForce, player.playerScriptableData.heightSpeedForce,
        player.playerScriptableData.boostTime,player.playerScriptableData.boostRefreshTime,player.playerScriptableData.cadence,player.playerScriptableData.damageDone,
        player.playerScriptableData.currentPoints,player.playerScriptableData.totalPoints,player.enemiesKilled,player.distanceTravelled,player.suppliesDroppedOnSpot); 
        formatter.Serialize(stream, dataSaved);
        stream.Close();
    }

    public static PlayerOnSaveData LoadData( )
    {
        string path = Application.persistentDataPath + "/PlayerDataSavedBinary.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            PlayerOnSaveData data= formatter.Deserialize(fileStream) as PlayerOnSaveData;
            fileStream.Close();
            return data;
        }
        else
        {
            Debug.LogError("File path dont exist in "+ path);
            return null;
        }
    }

    
    
    //planet
    public static void SaveLevelOfDestruction(ShapeSettings shape)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlanetDataSavedBinary.txt";
        FileStream stream = new FileStream(path, FileMode.Create);
        LevelOfDestructionData planetData = new LevelOfDestructionData(shape.noiseLayers[shape.noiseLayers.Length - 1]
            .noiseSettings.simpleNoiseSettings.minValue,GameManager.instance.supplyDropped);
        formatter.Serialize(stream, planetData);
        stream.Close();
    }
    public static LevelOfDestructionData LoadDataPlanet( )
    {
        string path = Application.persistentDataPath + "/PlanetDataSavedBinary.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            LevelOfDestructionData data= formatter.Deserialize(fileStream) as LevelOfDestructionData;
            fileStream.Close();
            return data;
        }
        else
        {
            Debug.LogError("File path dont exist in "+ path);
            return null;
        }
    }

    
    
    //enemy data
    public static void SaveEnemyData(Transform enemyContainer)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/EnemyData.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

 
        float[] pos=new float[enemyContainer.childCount*3];
        float[] rotation=new float[enemyContainer.childCount*4];
        for (int i = 0; i < enemyContainer.childCount; i+=3)
        {
          pos[i]=enemyContainer.GetChild(i).position.x;
          pos[i]=enemyContainer.GetChild(i).position.y;
          pos[i]=enemyContainer.GetChild(i).position.z;

        }
        for (int i = 0; i < enemyContainer.childCount; i+=4)
        {
            rotation[i]=enemyContainer.GetChild(i).rotation.x;
            rotation[i]=enemyContainer.GetChild(i).rotation.y;
            rotation[i]=enemyContainer.GetChild(i).rotation.z;
            rotation[i]=enemyContainer.GetChild(i).rotation.w;
        }

        EnemyData enemyData = new EnemyData(pos, rotation, enemyContainer.childCount);
        
        formatter.Serialize(stream, enemyData);
        stream.Close();
    }
    public static EnemyData LoadEnemyData( )
    {
        string path = Application.persistentDataPath + "/EnemyData.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            EnemyData data= formatter.Deserialize(fileStream) as EnemyData;
            fileStream.Close();
            return data;
        }
        else
        {
            Debug.LogError("File path dont exist in "+ path);
            return null;
        }
    }
}
