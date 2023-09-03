using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemyPatrol : MonoBehaviour
{
    public static Action OnShootEvent;
    //movement
    
    
    //Functs
    private bool randomBool
    {
        get { return (Random.value > 0.5f); }
    }
    public float radius;
    Vector3 gravityUp;
    public float turnAngle;
    public float forwardSpeed;
    [SerializeField]
    private float _defaultSpeed;
    [SerializeField]
    private float _boostSpeed;
    public float currentTimeBeforeShooting;

    float rightOrLeftRotationFactor;
    //references
    public Transform player;
    public ShooterController enemyShooterRef;
    //states
    public GameState State = GameState.onPatrol;
    

    
    //private values
    [SerializeField]
    private float patrolHeightTimer;

    [SerializeField] 
    private bool isGoingUp;
    //roll
    [SerializeField] 
    private float _rollForce;
    [SerializeField]
    private float _steeringForce;
    
    //height
    [SerializeField]
    private float _heightIncrementFactor;
    
    private float maxRadius;
    
    private float minRadius;
    
    [SerializeField]
    private float minDistanceToFollowPlayer;

    

    public AudioSource audioShoot;
    public AudioClip clip;

    private void Awake()
    {
        
    }

    void Start()
    {
        InitializateValues();
    }


    void InitializateValues()
    {
        radius = player.GetComponent<PlayerController>().radius;
        minRadius = player.GetComponent<PlayerController>().minRadius;
        maxRadius= player.GetComponent<PlayerController>().maxRadius;
        enemyShooterRef = GetComponent<ShooterController>();
        _defaultSpeed = forwardSpeed;
        isGoingUp = randomBool;
    }

    void Update()
    {


    }

    private void FixedUpdate()
    {
        if (GameManager.instance.currentState==GameManager.GameState.onGame)
        {
            currentTimeBeforeShooting += Time.deltaTime;
            checkPlayerDistance(player);
            if (State == GameState.onPatrol)
            {
                UpdateGameState(GameState.onPatrol);
            }
        }
    }

    float HandleRollAngle()
    {
        float rollRotation = rightOrLeftRotationFactor;
        turnAngle = rollRotation * _rollForce;
        return turnAngle;
    }

    public void checkPlayerDistance(Transform player) {
        float distanceToPlayer= Vector3.Distance(transform.position,player.position);

        if (distanceToPlayer>minDistanceToFollowPlayer) {
            //random movement
            UpdateGameState(GameState.onPatrol);
        }
        if (distanceToPlayer<minDistanceToFollowPlayer) {
            //follow
            UpdateGameState(GameState.playerInArea);
        }
        if (distanceToPlayer<minDistanceToFollowPlayer/1.1) {
            //when is close enought to player shoots
            shoot();
        }
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            //case when the player is not close
            case GameState.onPatrol:
                
                turnAngle = HandleRotation(turnAngle);
                UpdatePosition(forwardSpeed);
                turnAround(turnAngle);
                forwardSpeed = _defaultSpeed;
                enemyShooterRef.isShooting = false;
                HeightHandlerOnPatrol();
                HandleRollAngle();
                
                break;
            //case when player is in the trigger zone
            case GameState.playerInArea:
                HandleTrackPlayer();
                turnAngle = HandleRotation(turnAngle);
                UpdatePosition(forwardSpeed);
                turnAround(turnAngle);
                forwardSpeed = _boostSpeed;
                HeightHandlerOnPatrol();
                HandleRollAngle();

                //enemy needs to roll and increment heigh when the player is close 
                break;

        }

    }

    void HeightHandlerOnPatrol()
    {
        //both conditions are true in this case, needs to be fixed.
            //patrolHeightTimer += Time.deltaTime;
        //Mathf.Lerp(minRadius, maxRadius, (Mathf.Pow(Mathf.Sin(patrolHeightTimer * -0.01f), 2)));
        radius = player.GetComponent<PlayerController>().radius;    

    }

    void HandleFollowPlayerHeight()
    {
        if (radius<player.GetComponent<PlayerController>().radius)
        {
            radius += _heightIncrementFactor;
        }
        else if (radius>player.GetComponent<PlayerController>().radius)
        {
            radius -= _heightIncrementFactor;
        }else if (radius<=player.GetComponent<PlayerController>().radius-1||radius<=player.GetComponent<PlayerController>().radius+1)
        {
            radius = player.GetComponent<PlayerController>().radius;
        }
        
    }

    void HandleTrackPlayer() {
        Vector3 dirToTarget = player.position - transform.position;
        float angleBetweenEnemyAndPlayer = Vector3.SignedAngle(transform.forward, dirToTarget, transform.up);
        if (angleBetweenEnemyAndPlayer>0.1) {
            rightOrLeftRotationFactor = 1;
            _steeringForce = 5f;

        }
        else if (angleBetweenEnemyAndPlayer < 0.1)
        {
            rightOrLeftRotationFactor = -1;
            _steeringForce = 5f;

        } else if(angleBetweenEnemyAndPlayer > 0.1&& angleBetweenEnemyAndPlayer < 0.1)
        {
            _steeringForce = 0.4f;

            rightOrLeftRotationFactor = 0;
            
        }
    }

    float HandleRotation(float turnAngle)
    {
        float rightRotation = rightOrLeftRotationFactor;

        turnAngle = rightRotation * _steeringForce;
        return turnAngle;
    }
    public void turnAround(float turnAngle)
    {
        gravityUp = transform.position;
        transform.RotateAround(transform.position, gravityUp, turnAngle);
        transform.LookAt((transform.position + transform.forward * 10).normalized * (radius), gravityUp);
        transform.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;
    }

    public void UpdatePosition(float forwardSpeed)
    {
        Vector3 newPos = transform.position + transform.forward * forwardSpeed * Time.deltaTime;
        newPos = newPos.normalized * radius;

        transform.position = newPos;
    }
    public void shoot() {
          if (currentTimeBeforeShooting>GameManager.instance.maxTimeShootEnemy) {
                    enemyShooterRef.isShooting = true;
                    //spawn vfx
                    GetComponent<EnemyVFXController>().ShootVFXEvent();
                    audioShoot.PlayOneShot(clip);
                    currentTimeBeforeShooting= 0;
           }
    }

    public enum GameState
    {
        onPatrol,
        playerInArea,
    }



}

