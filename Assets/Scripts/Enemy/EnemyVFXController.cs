using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System;
public class EnemyVFXController : MonoBehaviour
{
    //it is good to implement an event here?
    public static Action OnShootEvent;
    public static Action OnHitEvent;

    public VisualEffect onShoot;
    public VisualEffect onHit;

    private void OnEnable()
    {
        OnHitEvent += OnHitVFXEvent;
        OnShootEvent += ShootVFXEvent;
        
    }

    private void OnDisable()
    {
        OnHitEvent -= OnHitVFXEvent;
        OnShootEvent -= ShootVFXEvent; 
    }

    void Start()
    {


    }

    void Update()
    {
        
    }

    public void ShootVFXEvent()
    {
        onShoot.Play();
        Debug.Log("shoot");
    }
    public void OnHitVFXEvent()
    {
        onHit.Play();
        Debug.Log("hit");

    }

}
