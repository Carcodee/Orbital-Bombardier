using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPreviewController : MonoBehaviour
{
    public PlayerStatsController playerRef;
    [Range(0f, 1f)]
    public float lineWidthStart;
    [Range(0f, 1f)]
    public float lineWidthEnd;


    public LineRenderer line;
    public LayerMask planetLayer;
    private void OnValidate()
    {
        line.startWidth = lineWidthStart;
        line.endWidth = lineWidthEnd;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (playerRef.playerMovementState==PlayerStatsController.MovementState.deployMode)
        {
            line.positionCount = 2; 
            CreateLine();
            
        }
        else if (playerRef.playerMovementState == PlayerStatsController.MovementState.normalMode)
        {
            line.positionCount = 0; ;
        }
 
    }
    public Vector3 RayToGround()
    {
        Vector3 dirToPlanet = Vector3.zero - transform.position;
        if (Physics.Raycast(transform.position,dirToPlanet,out RaycastHit hit, Mathf.Infinity,planetLayer))
        {

            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public void CreateLine()
    {
        
        line.startWidth = lineWidthStart;
        line.endWidth = lineWidthEnd;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, RayToGround());

    }
}
