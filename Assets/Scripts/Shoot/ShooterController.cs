using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnSpot;
    public bool isShooting;
    public float currentDamage;
    public Transform bulletContainer;
    void Start()
    {
        if (TryGetComponent<PlayerStatsController>(out PlayerStatsController player)) {
            currentDamage = player.damageDone;
        }
        if (TryGetComponent<EnemyStats>(out EnemyStats enemy)) {
            currentDamage = enemy.damageDone;
        }
    }

    void Update()
    {
        if (isShooting) {
            Shoot();
            isShooting = false;
        }
    }

    void Shoot() {
        if (this.TryGetComponent<PlayerController>(out PlayerController player))
        {
            GameObject bulletInstance = Instantiate(bullet, spawnSpot.position, transform.rotation, bulletContainer);
            BulletController bulletInstanceController = bulletInstance.GetComponent<BulletController>();
            bulletInstanceController.bulletDamage = currentDamage;
            Physics.IgnoreCollision(bulletInstance.GetComponent<Collider>(), player.GetComponent<Collider>());
            bulletInstanceController.radius = player.radius;

        }
        else if (this.TryGetComponent<EnemyPatrol>(out EnemyPatrol enemyCollider))
        {

            GameObject bulletInstance = Instantiate(bullet, spawnSpot.position, transform.rotation);
            BulletController bulletInstanceController = bulletInstance.GetComponent<BulletController>();
            bulletInstanceController.bulletDamage = currentDamage;
            Physics.IgnoreCollision(bulletInstance.GetComponent<Collider>(), enemyCollider.GetComponent<Collider>());
            bulletInstanceController.radius = enemyCollider.radius;
        }


    }


}
