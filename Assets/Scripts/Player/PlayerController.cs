using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData playerScriptableData;
    public GameController gameRef;
    public PlayerStatsController playerstatsRef;

    public bool isAlive = true;
    [Header("Planet")]
    public float radius;
    
    public float maxRadius, minRadius;
    [SerializeField]
    private float _heightIncrementFactor;


    [Header("Speed")]
    public float forwardSpeed;
    public float boostSpeed;
    [SerializeField]
    private float _defaultSpeed;
    Vector3 gravityUp;



    [Header("Rotation")]
    [SerializeField]
    private float turnAngle;
    [SerializeField]
    private float _steeringForce;



    [Header("Roll")]
    [SerializeField]
    private float _rollForce;




    [Header("Pitch")]
    [SerializeField]
    private float _pitchRotation, _pitchFactor;


    private void Awake()
    {

    }
    void Start()
    {

        playerstatsRef= GetComponent<PlayerStatsController>();
        playerScriptableData = playerstatsRef.playerScriptableData;
        BaseDataController.OnConfirmButtonEvent += SetRandomSpotOnSphere;
        PlayerStatsController.OnPlayerMovementValuesChange += InitializateValues;

        InitializateValues();

    }

    void Update()
    {





    }

    private void FixedUpdate()
    {
        if (GameManager.instance.currentState == GameManager.GameState.onGame)
        {
            if (isAlive == true)
            {

                turnAngle = HandleRotation();
                UpdatePosition(forwardSpeed);
                turnAround(turnAngle);
                HandleRollRotation(HandleRollAngle());
                HandleHeight();
                //HandlePitchRotation();
                //SetShipRotation();

            }
        }
    }

    public void InitializateValues()
    {

        _defaultSpeed = playerScriptableData.speed;
        forwardSpeed = playerScriptableData.speed;
        _heightIncrementFactor = playerScriptableData.heightSpeedForce;
        _steeringForce = playerScriptableData.turnForce;
        boostSpeed = playerScriptableData.boostSpeed ;
    }
    /// <summary>
    /// handle the turn rotation, like that is proportional the key
    /// </summary>
    /// <returns></returns>
    float HandleRotation()
    {
        float rightRotation = Input.GetAxis("Horizontal");
        turnAngle = rightRotation * _steeringForce;
        return turnAngle;
    }

    /// <summary>
    /// handle the turn rotation, like that is proportional the key
    /// </summary>
    /// <returns></returns>
    float HandleRollAngle()
    {
        float rollRotation = Input.GetAxis("Horizontal");
        turnAngle = rollRotation * _rollForce;
        return turnAngle;
    }

    public void SetRandomSpotOnSphere()
    {
        gameRef.SetOrientation(gameObject.transform);
    }
    void HandleHeight()
    {

        float vertical = Input.GetAxis("Vertical");
        if (vertical > 0)
        {
            radius += vertical * _heightIncrementFactor;

        }
        if (vertical < 0)
        {
            radius += vertical * _heightIncrementFactor;
        }
        radius = Mathf.Clamp(radius, minRadius, maxRadius);


    }

    void HandlePitchRotation()
    {
        float vertical = Input.GetAxis("Vertical");

        if (radius < maxRadius || radius > minRadius)
        {
            _pitchRotation = vertical * _pitchFactor;

        }
        if (radius == maxRadius || radius == minRadius)
        {
            _pitchRotation = 0;
        }

    }

    void SetShipRotation()
    {
        //transform.RotateAround(transform.position, transform.right, -_pitchRotation);
    }

    /// <summary>
    /// turns the player around the world based on the turn angle
    /// </summary>
    /// <param name="turnAngle"></param>
    public void turnAround(float turnAngle)
    {
        gravityUp = transform.position;
        transform.RotateAround(transform.position, gravityUp, turnAngle);
        transform.LookAt((transform.position + transform.forward * 10).normalized * (radius), gravityUp);
        transform.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;

    }

    /// <summary>
    /// control the position in the forward dir
    /// </summary>
    /// <param name="forwardSpeed"></param>
    public void UpdatePosition(float forwardSpeed)
    {
        Vector3 newPos = transform.position + transform.forward * forwardSpeed * Time.deltaTime;
        newPos = newPos.normalized * radius;

        transform.position = newPos;
    }

    public bool ApplyBoost(float input)
    {
        if (input == 0)
        {
            forwardSpeed = _defaultSpeed;
            return false;
        }
        else
        {

            forwardSpeed = boostSpeed;
            return true;
        }

    }

    public float defaultSpeed()
    {
        return forwardSpeed = _defaultSpeed;
    }
    /// <summary>
    /// handle the roll off the plane
    /// </summary>
    /// <param name="turnAngle">how much it will roll</param>
    public void HandleRollRotation(float turnAngle)
    {


        transform.RotateAround(transform.position, transform.forward, -turnAngle);

    }


}
