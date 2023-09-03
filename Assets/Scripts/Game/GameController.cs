using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController : MonoBehaviour
{
    
    public float currentTime;
    public GameObject positionToDeliver;
    public Transform planet;
    public float offset=1;
    public LayerMask planetLayer;
    public Planet planetRef;
    
    void Start()
    {
        planetRef.GeneratePlanet();
        SetOrientation(positionToDeliver.transform);

    }

    void Update()
    {

        currentTime += Time.deltaTime;
        if (GameManager.instance.currentState==GameManager.GameState.onGame && GameManager.instance.currentRoundTime>GameManager.instance.timeToDeliver) {
            SetOrientation(positionToDeliver.transform);
            positionToDeliver.transform.up = -(transform.position - positionToDeliver.transform.position);
            //resets the timer values
            GameManager.instance.timeToDeliver = Random.Range(20f, 40f);
            GameManager.instance.currentRoundTime = 0;
        }
    }
    public Vector3 GenerateRandomPointOnSphere() {

        Vector3 position;
        return position = Random.insideUnitSphere;
    }
    public void SetOrientation(Transform objectOriented) {
        Vector3 spawnPos;
        Vector3 randomPointOnSphere = GenerateRandomPointOnSphere();
        Vector3 dir = randomPointOnSphere.normalized - transform.position;
        Ray rayDir = new Ray(transform.position,dir.normalized*offset);
        Ray rayDirFromOutside=new Ray();
        rayDirFromOutside.origin = rayDir.GetPoint(offset);
        rayDirFromOutside.direction = -rayDir.direction;
        Vector3 newDir = rayDirFromOutside.GetPoint(offset) - rayDirFromOutside.GetPoint(0);
        Debug.DrawRay(rayDirFromOutside.GetPoint(0),newDir,Color.red,50f);
        RaycastHit hit;
        
        if (Physics.Raycast(rayDirFromOutside.GetPoint(0),newDir, out hit, Mathf.Infinity,planetLayer))
        {

            spawnPos = hit.point;
            objectOriented.position = spawnPos;
            objectOriented.transform.up = -(transform.position - objectOriented.transform.position);
            
        }
        else
        {
            spawnPos = Vector3.zero;
            Debug.LogWarning("unable to to make the orientation");
           

        }


    }

}
