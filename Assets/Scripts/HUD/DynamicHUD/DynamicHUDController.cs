using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DynamicHUDController : MonoBehaviour
{
    [Header("Crosshair tracker")]
    public TextMeshProUGUI heighTracker;
    public GameObject crosshair;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    public Vector3 offset;

    public Transform enemiesContainer;

    public EnemyPatrol[] enemies;
    private EnemyPatrol closerEnemy;
    public PlayerController playerRef;
    void Start()
    {
        crosshair.SetActive(false);
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse1))
        {

            HandleCrossHair();
        }
        else
        {
            crosshair.SetActive(false);
        }

    }
    public EnemyPatrol[] GetAllEnemies()
    {
        List<EnemyPatrol> list = new List<EnemyPatrol>();
        if (enemiesContainer.childCount != enemies.Length)
        {
            foreach (Transform child in enemiesContainer)
            {
                list.Add(child.GetComponent<EnemyPatrol>());

            }
            return list.ToArray();
        }
        else
        {
            return enemies;
        }

    }

    public void HandleCrossHair()
    {
        crosshair.SetActive(true);
        enemies = GetAllEnemies();
        closerEnemy = CheckCloserEnemy(ref enemies);
        if (closerEnemy != null)
        {
            Vector3 pos = mainCamera.WorldToScreenPoint(closerEnemy.transform.position + offset);
            heighTracker.text = "Height " + closerEnemy.radius.ToString();
            if (closerEnemy.radius > playerRef.radius - 10 && closerEnemy.radius < playerRef.radius + 10)
            {
                heighTracker.color = Color.green;
            }
            else
            {
                heighTracker.color = Color.white;
            }
            if (crosshair.transform.position != pos)
            {
                crosshair.transform.position = pos;
            }
        }
    }
    public EnemyPatrol CheckCloserEnemy(ref EnemyPatrol[] enemies)
    {

        float closerDistanceToTarget = 9999999;
        EnemyPatrol closerEnemy = null;
        for (int i = 0; i < enemies.Length; i++)
        {
            //checks if targets are inside the camera frustum
            Vector3 screenPoint = mainCamera.WorldToViewportPoint(enemies[i].transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            float dstTotarget = Vector3.Distance(playerRef.transform.position, enemies[i].transform.position);
            if (closerDistanceToTarget > dstTotarget && onScreen)
            {
                closerDistanceToTarget = dstTotarget;
                closerEnemy = enemies[i];
            }
        }
        if (closerEnemy == null)
        {
            Debug.LogWarning("detection target failed");
            return null;
        }
        return closerEnemy;
    }

}
