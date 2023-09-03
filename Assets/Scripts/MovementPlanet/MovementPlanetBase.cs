using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementPlanetBase : MonoBehaviour
{
    [Header("Planet")]
    public float radius;
    [SerializeField]
    private float maxRadius, minRadius;
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
    public float turnAngle;
    [SerializeField]
    private float _steeringForce;



    [Header("Roll")]
    [SerializeField]
    private float _rollForce;




    [Header("Pitch")]
    [SerializeField]
    private float _pitchRotation, _pitchFactor;
    void Start()
    {

    }

    void Update()
    {
        
    }
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
    /// <summary>
    /// handle the roll off the plane
    /// </summary>
    /// <param name="turnAngle">how much it will roll</param>
    public void HandleRollRotation(float turnAngle)
    {


        transform.RotateAround(transform.position, transform.forward, -turnAngle);

    }
}
