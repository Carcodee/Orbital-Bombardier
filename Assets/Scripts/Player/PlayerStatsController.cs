using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;

public class PlayerStatsController : MonoBehaviour, IDamageable, ISaveable 
{
    
    public static Action OnHitEvent;
    public delegate float OnHealthChange();
    public static OnHealthChange onHealthChange;
    public delegate int OnScoreChange();
    public static OnScoreChange onScoreChange;
        
    //this is the player scriptable data that is given to the rest of the clases
    public PlayerData playerScriptableData;

    public PlayerData playerNewGameScriptableData;
    //Data that save the normal values of the ship
    public PlayerScriptableBuffer playerScriptableBuffer;
    //Controls the movement type of the ship
    public MovementState playerMovementState;
    //eventt that is called when is wanted to change the stats values
    public static Action OnPlayerMovementValuesChange;

    [Header("spawn data")] public Vector3 spawnSpot;
    
    
    [Header("Health data")]
    public float health;
    
    public IDamageable iDamageableInterface;
    
    public float damageDone = 40;
    //refs
    public ShooterController shootRef;

    [Header("Audio")]
    public AudioSource audioRef;
    public AudioClip hit;
    
    [Header("Deploy data")]
    public static float dstToTarget;
    public GameObject zoneToDeploy;
    public int playerDroppableSupplies;
    public float weight;

    [Header("Boost Stats")]
    public float currentTimeBooster;
    
    [Header("Current Points of the player")]
    public int currentPoints;

    [Header("End game data")]
    public float distanceTravelled;
    public int enemiesKilled;
    public int totalPoints;
    public int suppliesDroppedOnSpot;

    private void Awake()
    {

        //all data should be load it at the same time
        if (GameManager.instance.isNewGame)
        {
            SetDataNewGame();
        }
        else 
        {
            LoadData();
        }
        //buffered data
        SaveBufferedPlayerData();
        //start values
        InitializateStats();
        OnPlayerMovementValuesChange += InitializateStats;
    }
    void Start() {


        playerMovementState = MovementState.normalMode;
        shootRef = GetComponent<ShooterController>();
        iDamageableInterface = GetComponent<IDamageable>();
        OnPlayerMovementValuesChange?.Invoke();
        onHealthChange += ReturnHP;
        onScoreChange += GetTotalPoints;
    }

    private void OnEnable()
    {
        EnemyStats.OnEnemyDead += AddEnemyKilled;
        SupplyPrefabController.OnSupplyDropped += AddSupplyDropped;

    }
    private void OnDisable()
    {
        EnemyStats.OnEnemyDead -= AddEnemyKilled;
        SupplyPrefabController.OnSupplyDropped -= AddSupplyDropped;
    }
    void Update() {
        if (GameManager.instance.currentState == GameManager.GameState.onGame)
        {
            StoreDstTravelled();
            currentTimeBooster += Time.deltaTime;
            //distance to supply drop
            dstToTarget = Vector3.Distance(transform.position, zoneToDeploy.transform.position);
            if (Input.GetKeyDown(KeyCode.E) && playerMovementState == MovementState.normalMode)
            {
                MovementeStateUpdater(MovementState.deployMode);
                return;
            }
            if (Input.GetKeyDown(KeyCode.E) && playerMovementState == MovementState.deployMode)
            {
                MovementeStateUpdater(MovementState.normalMode);
                return;
            }

        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MeshPlanetTag"))
        {

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("MeshPlanetTag"))
        {
            Debug.Log("hit");
            TakeDamage(health);
            InitializateStats();

        }
    }
    public void AddEnemyKilled()
    {
        enemiesKilled++;
    }
    public void AddSupplyDropped()
    {
        suppliesDroppedOnSpot++;
    }
    private void StoreDstTravelled()
    {
        distanceTravelled += Time.deltaTime * playerScriptableData.speed;
    }

    public float ReturnHP()
    {
        return health;
    }
    public int GetTotalPoints()
    {
        return totalPoints;
    }

    public void SetDataNewGame()
    {
        transform.position = spawnSpot;
        transform.rotation =quaternion.identity;
        //scriptable object;

        playerScriptableData.health = playerNewGameScriptableData.health;
        playerScriptableData.suppliesOnCharge =  playerNewGameScriptableData.suppliesOnCharge;
        playerScriptableData.speed =  playerNewGameScriptableData.speed;
        playerScriptableData.boostSpeed =  playerNewGameScriptableData.boostSpeed;
        playerScriptableData.turnForce =  playerNewGameScriptableData.turnForce;
        playerScriptableData.heightSpeedForce =  playerNewGameScriptableData.heightSpeedForce;
        playerScriptableData.boostTime =  playerNewGameScriptableData.boostTime;
        playerScriptableData.boostRefreshTime =  playerNewGameScriptableData.boostRefreshTime;
        playerScriptableData.cadence =  playerNewGameScriptableData.cadence;
        playerScriptableData.damageDone =  playerNewGameScriptableData.damageDone;
        playerScriptableData.currentPoints =  playerNewGameScriptableData.currentPoints;
        playerScriptableData.totalPoints = 0;
        OnPlayerMovementValuesChange?.Invoke();
        Debug.Log("new game");
    }
    public void LoadData()
    {
        Vector3 pos;
        pos.x= SaveSystem.LoadData().pos[0];
        pos.y= SaveSystem.LoadData().pos[1];
        pos.z= SaveSystem.LoadData().pos[2];

        Quaternion rotation;
        rotation.x = SaveSystem.LoadData().rotation[0];
        rotation.y = SaveSystem.LoadData().rotation[1];
        rotation.z = SaveSystem.LoadData().rotation[2];
        rotation.w = SaveSystem.LoadData().rotation[3];

        //scriptable object data
        playerScriptableData.health = SaveSystem.LoadData().health;
        playerScriptableData.suppliesOnCharge = SaveSystem.LoadData().suppliesOnCharge;
        playerScriptableData.speed = SaveSystem.LoadData().speed;
        playerScriptableData.boostSpeed = SaveSystem.LoadData().boostSpeed;
        playerScriptableData.turnForce = SaveSystem.LoadData().turnForce;
        playerScriptableData.heightSpeedForce = SaveSystem.LoadData().heightIncrementForce;
        playerScriptableData.boostTime = SaveSystem.LoadData().boostTime;
        playerScriptableData.boostRefreshTime = SaveSystem.LoadData().boostRefreshTime;
        playerScriptableData.cadence = SaveSystem.LoadData().cadence;
        playerScriptableData.damageDone = SaveSystem.LoadData().damageDone;
        playerScriptableData.currentPoints = SaveSystem.LoadData().currentPoints;
        playerScriptableData.totalPoints = SaveSystem.LoadData().totalPoints;
        distanceTravelled = SaveSystem.LoadData().dstTravelled;
        enemiesKilled = SaveSystem.LoadData().enemyKilled;
        suppliesDroppedOnSpot = SaveSystem.LoadData().supplieDroppedOnSpot;
        //pos and rotation
        transform.position = pos;
        transform.rotation = rotation;
        OnPlayerMovementValuesChange?.Invoke();
        Debug.Log("load it");
    }
    
    
    
    public void SaveData()
    {
        SaveSystem.SaveData(gameObject.GetComponent<PlayerStatsController>());
    }
    /// <summary>
    /// Set data to from the scriptable object
    /// </summary>
    public void InitializateStats()
    {
        if (playerMovementState==MovementState.normalMode)
        {
            ScriptableValuesChanged();
        }
        
        weight = playerScriptableData.weight;
        currentPoints = playerScriptableData.currentPoints; 
        health = playerScriptableData.health; 
        damageDone = playerScriptableData.damageDone; 
        playerDroppableSupplies = playerScriptableData.suppliesOnCharge;
        totalPoints = playerScriptableData.totalPoints;
        
        //new data is saved 
    }
    public void ScriptableValuesChanged()
    {
        //needs to be fixed, speed is going to negative values.
        playerScriptableData.weight = (1000 * playerScriptableData.suppliesOnCharge);
        playerScriptableData.speed = playerScriptableBuffer.speed - (playerScriptableData.weight / 200);
        playerScriptableData.boostSpeed = playerScriptableBuffer.boostSpeed - (playerScriptableData.weight / 200);
        if (playerScriptableData.speed<20f)
        {
            playerScriptableData.speed = 20;
            playerScriptableData.boostSpeed = playerScriptableBuffer.boostSpeed - (playerScriptableData.weight / 200);

        }
    }

    public void SaveBufferedPlayerData()
    {
        playerScriptableBuffer = new PlayerScriptableBuffer(playerScriptableData.turnForce, playerScriptableData.cadence,
        playerScriptableData.heightSpeedForce, playerScriptableData.speed,playerScriptableData.boostSpeed);
    }
    /// <summary>
    /// player takes damage
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage) {
        if (health <= 0) {
            GameManager.instance.GameStateUpdater(GameManager.GameState.ended);

        }
        else {
            playerScriptableData.health -= (int)damage;
            health = playerScriptableData.health;
            CameraShaker.instance?.GenerateShake(2f,2f,0.5f);
            audioRef.PlayOneShot(hit);

            OnHitEvent?.Invoke();
        }

    }
    public enum MovementState
    {
        normalMode,
        deployMode
    }


    /// <summary>
    /// simple state machine to handle the movement states
    /// </summary>
    /// <param name="newMovement"></param>
    public void MovementeStateUpdater(MovementState newMovement)
    {
        playerMovementState = newMovement;
        switch (newMovement)
        {
            case MovementState.normalMode:
                
                SetNormalModeValues(playerScriptableData);
                
                OnPlayerMovementValuesChange?.Invoke();
                break;
            case MovementState.deployMode:
                SetDeployValues(playerScriptableData);
                OnPlayerMovementValuesChange?.Invoke();

                break;
        }
    }

    /// <summary>
    /// Set the deploy values to the player data
    /// </summary>
    /// <param name="dataToTransform"></param>
    public void SetDeployValues(PlayerData dataToTransform)
    {
        dataToTransform.turnForce = 6;
        dataToTransform.cadence = 100;
        dataToTransform.heightSpeedForce = 3;
        dataToTransform.speed = 30;
        
        
    }

    /// <summary>
    /// Return the values to normal values
    /// </summary>
    /// <param name="dataToTransform"></param>
    public void SetNormalModeValues(PlayerData dataToTransform)
    {
        dataToTransform.turnForce = playerScriptableBuffer.turnForce;
        dataToTransform.cadence = playerScriptableBuffer.cadence;
        dataToTransform.heightSpeedForce = playerScriptableBuffer.heightSpeedForce;
        dataToTransform.speed = playerScriptableBuffer.speed;
        dataToTransform.boostSpeed= playerScriptableBuffer.boostSpeed;
    }
    public struct PlayerScriptableBuffer{
        public float turnForce;
        public float cadence;
        public float heightSpeedForce;
        public float speed;
        public float boostSpeed;

        public PlayerScriptableBuffer(float turnForce, float cadence, float heightSpeedForce, float speed,float boostSpeed)
        {
            this.turnForce = turnForce;
            this.cadence = cadence;
            this.heightSpeedForce = heightSpeedForce;
            this.speed = speed;
            this.boostSpeed = boostSpeed;
        }
    }
    private void OnApplicationQuit()
    {
        SetNormalModeValues(playerScriptableData);
        SaveData();
    }
}

