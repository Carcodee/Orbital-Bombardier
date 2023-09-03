using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXController : MonoBehaviour
{

    public VisualEffect onShoot;
    public VisualEffect onHit;
    void Start()
    {
        PlayerStatsController.OnHitEvent += OnHitVFXEvent;
        PlayerInputController.OnShootEvent += ShootVFXEvent;
    }

    void Update()
    {
        
    }

    public void ShootVFXEvent()
    {
        onShoot.Play();
    }
    public void OnHitVFXEvent()
    {
        onHit.Play();
    }
    
}
