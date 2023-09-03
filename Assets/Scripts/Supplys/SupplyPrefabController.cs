using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SupplyPrefabController : MonoBehaviour
{
    public static Action OnSupplyDropped;
    public float gravitySpeed;
    public GameController game;
    public PlayerData scriptableObj;
    void Start()
    {
        game = FindObjectOfType<GameController>();
      StartCoroutine(destroySupply());

    }

    void Update()
    {
        FallSupply();
    }
    public void FallSupply() {
        Vector3 gravityUp = -transform.up;
        transform.position += gravityUp * gravitySpeed * Time.deltaTime;
        }
    IEnumerator destroySupply() {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SupplyZone"))
        {
            
            scriptableObj.currentPoints++;
            scriptableObj.totalPoints+=150;
            PlayerStatsController.OnPlayerMovementValuesChange?.Invoke();
            MenuController.OnScoreChangeHud?.Invoke();
            OnSupplyDropped?.Invoke();

        }
        game.SetOrientation(other.transform);
    }
}
