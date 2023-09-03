using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyStats : MonoBehaviour, IDamageable
{
    public static Action OnEnemyDead;
    public static Action OnHitEvent;
    public float health=100f;
    public float damageDone=10;
    public float shootCd;
    public ShooterController shootRef;
    public IDamageable iDamageableInterface;
    public AudioSource audioRef;
    public AudioClip [] hit;
    void Start()
    {
        shootRef = GetComponent<ShooterController>();
        iDamageableInterface = GetComponent<IDamageable>();
        audioRef = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    
    public void TakeDamage(float damage) {
        if (health<=0)
        {
            OnEnemyDead?.Invoke();
            GetComponent<VFXSpawner>().GenerateDeadExplotion();
            audioRef.PlayOneShot(hit[0]);
            gameObject.SetActive(false);
            return;
        } else {
            health -= damage;
            audioRef.PlayOneShot(hit[1]);
            GetComponent<EnemyVFXController>().OnHitVFXEvent();
        }

    }
    
}
