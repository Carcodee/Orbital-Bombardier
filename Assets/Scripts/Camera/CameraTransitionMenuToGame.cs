using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraTransitionMenuToGame : MonoBehaviour
{
    [SerializeField]
    public CinemachineVirtualCamera gameCam;
    [SerializeField]
    public CinemachineVirtualCamera menuCam;

    private void OnEnable()
    {
        StartCoroutine(WaitToChangeCamera(0.5f));
    }
    

    void Start()
    {
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            MoveCamera();
        }
    }

    public void MoveCamera()
    {
        //cam changed to menu
        if (menuCam.Priority==10)
        {
            gameCam.Priority = 10;
            menuCam.Priority = 20;
            return;
        }
        //cam changed to menu
        if (gameCam.Priority==10)
        {
            gameCam.Priority = 20;
            menuCam.Priority = 10;
            return;
        }

    }

    IEnumerator WaitToChangeCamera(float time)
    {
        yield return new WaitForSeconds(time);
        MoveCamera();
            
    }
}
