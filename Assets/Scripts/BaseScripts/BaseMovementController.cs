using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovementController :MovementPlanetBase
{

    void Start()
    {
        
    }


    void Update()
    {
        if (GameManager.instance.currentState==GameManager.GameState.onGame)
        {
            UpdatePosition(forwardSpeed);
            turnAround(turnAngle);
        }
    }
}
