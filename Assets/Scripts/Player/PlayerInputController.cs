using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PlayerInputController : MonoBehaviour
{
    public static Action OnShootEvent;
    public PlayerData playerScriptableData;
    public ShooterController playerShooterController;
    public float cadence;
    private float currentTimeToShoot;
    public AudioSource audioRef;
    public AudioClip[] clip;
    //UI
    public Camera cam;
    private void Awake()
    {
        playerScriptableData = GetComponent<PlayerStatsController>().playerScriptableData;
        PlayerStatsController.OnPlayerMovementValuesChange += InitializateValues;
    }
    void Start()
    {
        InitializateValues();
        playerShooterController = GetComponent<ShooterController>();
    }
    public void InitializateValues()
    {
        cadence = playerScriptableData.cadence;
    }

    void Update()
    {
        if (GameManager.instance.currentState==GameManager.GameState.onGame)
        {
            currentTimeToShoot += Time.deltaTime;



            float fire = Input.GetAxis("Fire1");
            if (fire == 1 && currentTimeToShoot > cadence)
            {
                CameraShaker.instance.GenerateShake(1f,1f,0.5f);
                audioRef.PlayOneShot(clip[0]);
                currentTimeToShoot = 0;
                playerShooterController.isShooting = true;
                OnShootEvent?.Invoke();
            }
        }

    }

}
