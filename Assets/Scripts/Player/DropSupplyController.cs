using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSupplyController : MonoBehaviour
{
    public SupplyPrefabController supply;
    public PlayerData scriptablePlayerData;
    public Transform spawnSpot;
    public MenuController menuRef;
    public AudioSource noBombsAudio;
    private void Awake()
    {
    }
    void Start()
    {
        scriptablePlayerData = GetComponent<PlayerStatsController>().playerScriptableData;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DropSupply();
        }
    }
    public void DropSupply() {
        if (scriptablePlayerData.suppliesOnCharge > 0) { 
        SupplyPrefabController supplyPrefab = Instantiate(supply,spawnSpot.position,transform.rotation);
        supplyPrefab.scriptableObj = scriptablePlayerData;
            scriptablePlayerData.suppliesOnCharge--;
            PlayerStatsController.OnPlayerMovementValuesChange?.Invoke();
        }
        else
        {
            menuRef.BombText(true);
            Debug.Log("You need to refill the suplies");
        }
    }
}
