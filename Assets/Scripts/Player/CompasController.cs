using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompasController : MonoBehaviour
{
    public Transform centerOfTheSphere;
    public Transform target;
    public Transform player;
    public Transform cam;
    private void OnValidate()
    {

    }
    void Start()
    {
        
    }

    void Update()
    {
        SetDirection();

    }
    Vector3 CalculateDirToPointInSphere(Vector3 pointA,Vector3 pointB)
    {
        Vector3 sphereCenter = Vector3.zero;
        Vector3 centerToB = pointB - sphereCenter;
        Vector3 centerToA = pointA - sphereCenter;
        Vector3 crossVectorPlayerTarget = Vector3.Cross(centerToA,centerToB).normalized;
        return Vector3.Cross(crossVectorPlayerTarget, centerToA);
    }
    void SetDirection()
    {
        Vector3 dirToTarget =  CalculateDirToPointInSphere(player.position, target.position);
        float angleToDir = Vector3.SignedAngle(player.forward, dirToTarget,-player.up);
        float anglePlayerRelativeToCamera = Vector3.SignedAngle(cam.transform.up, player.forward,-player.up);
        transform.eulerAngles = new Vector3(0 , angleToDir + anglePlayerRelativeToCamera, 0);
        transform.forward = dirToTarget;


    }
}
