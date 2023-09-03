using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BulletController : MonoBehaviour
{
    public float bulletDamage;
    Vector3 gravityUp;
    public float radius;
    public float forwardSpeed;
    
    void Start()
    {
        StartCoroutine(destroyBullet());
        
    }

    void Update()
    {
        UpdatePosition(forwardSpeed);
        turnAround(0);
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
    /// turns the player around the world based on the turn angle
    /// </summary>
    /// <param name="turnAngle"></param>
    public void turnAround(float turnAngle)
    {
        gravityUp = transform.position;
        transform.RotateAround(transform.position, gravityUp, turnAngle);
        transform.LookAt((transform.position + transform.forward * 10).normalized * (radius), gravityUp);
        transform.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;
    }


    private void OnTriggerEnter(Collider other) {
        bool isDamageable = other.TryGetComponent<IDamageable>(out IDamageable idamageableEntity);
        if (isDamageable) {
            idamageableEntity.TakeDamage(bulletDamage);
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision) {

    }
    IEnumerator destroyBullet() {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
