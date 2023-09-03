using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class BaseDataController : MonoBehaviour
{
    public GameObject UI;
    //event that is called to set values
    public static Action OnConfirmButtonEvent;
    public PlayerData playerScriptableObject;
    public Transform playerTransform;
    public GameController gameRef;
    public int totalPoints;
    [Header("UI elements/Changeables")]
    public TextMeshProUGUI [] upgradeables;
    [Header("UI elements/Base Stats")]
    public TextMeshProUGUI [] statsOnScreen;
    public PlayerDataBeforeConfirm playerNotSavedData;
    public TextMeshProUGUI points;
    public GameObject baseUI;

    private void OnEnable()
    {

    }
    void Start()
    {

    }

    void Update()
    {

        if (GameManager.instance.currentState == GameManager.GameState.baseZone)
        {
            baseUI.SetActive(true);
            if (baseUI.activeSelf)
            {
                points.text = "AVALIBLE POINTS: " + playerNotSavedData.currentPoints;
            }
        }
        else
        {
            baseUI.SetActive(false);
        }

    }
    public void SetDataPlayerScriptableObject()
    {
        playerNotSavedData = new PlayerDataBeforeConfirm(playerScriptableObject.health, playerScriptableObject.suppliesOnCharge, playerScriptableObject.speed, playerScriptableObject.weight,
playerScriptableObject.turnForce, playerScriptableObject.heightSpeedForce, playerScriptableObject.boostTime, playerScriptableObject.boostRefreshTime,
playerScriptableObject.cadence, playerScriptableObject.damageDone, playerScriptableObject.currentPoints,playerScriptableObject.boostSpeed);
    }

    public void OpenWindow()

    {
        totalPoints = playerNotSavedData.currentPoints;
    }

    public void InitializateStats()
    {
        totalPoints = playerNotSavedData.currentPoints;
        statsOnScreen[0].text = "WEIGHT: " + playerNotSavedData.weight;
        statsOnScreen[1].text = "CADENCE: " + playerNotSavedData.cadence;
        statsOnScreen[2].text = "HEALTH: " + playerNotSavedData.health+"%";
        statsOnScreen[3].text = "BOOST TIME: " + playerNotSavedData.boostTime;
        statsOnScreen[4].text = "TURN FORCE: " + playerNotSavedData.turnForce;
        statsOnScreen[5].text = "BOOST REFRESH TIME: " + playerNotSavedData.boostRefreshTime;
        statsOnScreen[6].text = "HEIGHT INCREMENT FORCE: " + playerNotSavedData.heightSpeedForce;
        statsOnScreen[7].text = "SPEED: " + playerNotSavedData.speed + "/ (Real speed) : " + (playerNotSavedData.speed - (playerNotSavedData.suppliesOnCharge * 1000) / 200);
        statsOnScreen[8].text = "SUPLIES: " + playerNotSavedData.suppliesOnCharge;
        statsOnScreen[9].text = "BOOST SPEED: " + playerNotSavedData.boostSpeed+ "/ (Real BOOST SPEED) : " + (playerNotSavedData.boostSpeed - playerNotSavedData.weight/200);;


    }
    public void RestoreHP()
    {
        if (playerNotSavedData.currentPoints > 3 && playerNotSavedData.health<100)
        {
            playerNotSavedData.health = 100;
            playerNotSavedData.currentPoints-=3;
            statsOnScreen[2].text = "HEALTH: " + playerNotSavedData.health + "%";

        }
        else
        {
            Debug.Log("Non posible change Health");
        }
    }
    public void OnPointAdded(int index)
    {
        if (playerNotSavedData.currentPoints>0||index==0)
        {
            if (ModifyStatsAdd(index))
            {
                
            }
            else
            {
                Debug.Log("Non posible change");
            }
        }
    }

    public void OnPointSustracted(int index)
    {
      
        if (playerNotSavedData.currentPoints<totalPoints||index==0)
        {
            if (ModifyStatsSustract(index))
            {
                
            }
            else
            {
                Debug.Log("Non posible change");
            }

        }
    }
    public bool ModifyStatsAdd(int index)
    {
        //if (playerNotSavedData.speed<=0||playerNotSavedData.boostRefreshTime<=0 || playerNotSavedData.boostTime<=0.01 ||playerNotSavedData.cadence<=0)
        //{
        //    Debug.Log("Not posible value");

        //    return;
        //}
        switch (index)
        {
            case 0:
                playerNotSavedData.suppliesOnCharge++;
                float weight = (playerNotSavedData.suppliesOnCharge*1000);
                if (weight<0 || playerNotSavedData.speed - weight / 200 < 0)
                {
                    playerNotSavedData.suppliesOnCharge--;
                    return false;   
                }
                playerNotSavedData.weight = weight;
                statsOnScreen[7].text = "SPEED: " + playerNotSavedData.speed + "/ (Real speed) : " + (playerNotSavedData.speed - weight/200);
                statsOnScreen[8].text = "SUPLIES: " + playerNotSavedData.suppliesOnCharge;
                statsOnScreen[0].text = "WEIGHT: " + weight;

                return true;
            case 1:
                float cadence = playerNotSavedData.cadence - 0.01f;
                if (cadence<=0)
                {
                    return false;
                }
                statsOnScreen[1].text = "CADENCE: " + cadence;
                playerNotSavedData.cadence = cadence;
                playerNotSavedData.currentPoints--;

                return true;
            case 2:
                float turnForce = playerNotSavedData.turnForce + 1;
                statsOnScreen[4].text="TURN FORCE: " + turnForce;
                playerNotSavedData.turnForce = turnForce;
                playerNotSavedData.currentPoints--;

                return true;
            case 3:
                float boostTime = playerNotSavedData.boostTime - 0.01f;
                if (boostTime<=0)
                {
                    return false;
                }
                statsOnScreen[3].text = "BOOST TIME: " + boostTime;
                playerNotSavedData.boostTime = boostTime;
                playerNotSavedData.currentPoints--;

                return true;
            case 4:
                float boostCD = playerNotSavedData.boostRefreshTime + 0.01f;
                statsOnScreen[5].text = "BOOST REFRESH TIME: " + boostCD;
                playerNotSavedData.boostRefreshTime = boostCD;
                playerNotSavedData.currentPoints--;

                return true;
            case 5:
                int health = playerNotSavedData.health + 20;
                statsOnScreen[2].text = "HEALTH: " + health;
                playerNotSavedData.health = health;
                playerNotSavedData.currentPoints--;

                return true;
            case 6:
                float heightIncrementForce = playerNotSavedData.heightSpeedForce + 0.1f;
                statsOnScreen[6].text = "HEIGHT INCREMENT FORCE: " + heightIncrementForce;
                playerNotSavedData.heightSpeedForce = heightIncrementForce;
                playerNotSavedData.currentPoints--;

                return true;
            case 7:
                float speed=playerNotSavedData.speed+5f;
                playerNotSavedData.speed = speed;
                statsOnScreen[7].text = "SPEED: " + playerNotSavedData.speed.ToString()+ "/ (Real speed) : " + (playerNotSavedData.speed - playerNotSavedData.weight/200);
                playerNotSavedData.currentPoints--;
                return true;
            case 8:
                float boost=playerNotSavedData.boostSpeed+10f;
                playerNotSavedData.boostSpeed = boost;
                statsOnScreen[9].text = "BOOST SPEED: " + playerNotSavedData.boostSpeed.ToString()+ "/ (Real BOOST) : " + (playerNotSavedData.boostSpeed - playerNotSavedData.weight/200);
                playerNotSavedData.currentPoints--;
                return true;
        }
        
        return false;
    }
    public bool ModifyStatsSustract(int index)
    {
        switch (index)
        {//To fix
            case 0:
                playerNotSavedData.suppliesOnCharge--;
                float weight =(playerNotSavedData.suppliesOnCharge * 1000);
                if (playerNotSavedData.suppliesOnCharge<1)
                {
                    playerNotSavedData.suppliesOnCharge++;
                    return false;
                }
                playerNotSavedData.weight = weight;
                

                statsOnScreen[0].text = "WEIGHT: " + weight;
                statsOnScreen[7].text = "SPEED: " + playerNotSavedData.speed + "/ (Real speed) : " + (playerNotSavedData.speed - weight / 200);
                statsOnScreen[8].text = "SUPLIES: " + playerNotSavedData.suppliesOnCharge;
                return true;
            case 1:
                float cadence = playerNotSavedData.cadence + 0.01f;
                statsOnScreen[1].text = "CADENCE: " + cadence;
                playerNotSavedData.cadence = cadence;
                playerNotSavedData.currentPoints++;
                return true;
            case 2:
                float turnForce = playerNotSavedData.turnForce - 1;
                if (turnForce <= 0)
                {
                    return false;
                }
                statsOnScreen[4].text = "TURN FORCE: " + turnForce;
                playerNotSavedData.turnForce = turnForce;
                playerNotSavedData.currentPoints++;
                return true;
            case 3:
                float boostTime = playerNotSavedData.boostTime + 0.01f;
                statsOnScreen[3].text = "BOOST TIME: " + boostTime;
                playerNotSavedData.boostTime = boostTime;
                playerNotSavedData.currentPoints++;
                return true;
            case 4:
                float boostCD = playerNotSavedData.boostRefreshTime - 0.01f;
                if (boostCD<= 0)
                {
                    return false;
                }
                statsOnScreen[5].text = "BOOST REFRESH SPEED: " + boostCD;
                playerNotSavedData.boostRefreshTime = boostCD;
                playerNotSavedData.currentPoints++;
                return true;
            case 5:
                int health = playerNotSavedData.health - 20;
                if (health<= 0)
                {
                    return false;
                }
                statsOnScreen[3].text = "HEALTH: " + health;
                playerNotSavedData.health = health;
                playerNotSavedData.currentPoints++;
                return true;
            case 6:
                float heightIncrementForce = playerNotSavedData.heightSpeedForce - 0.1f;
                if (heightIncrementForce<=0)
                {
                    return false;
                }
                statsOnScreen[6].text = "HEIGHT INCREMENT FORCE: " + heightIncrementForce;
                playerNotSavedData.heightSpeedForce = heightIncrementForce;
                playerNotSavedData.currentPoints++;
                return true;
            case 7:
                if (playerNotSavedData.speed<20)
                {
                    print("you cant reduce more the speed");
                    return false;
                }
                float speed=playerNotSavedData.speed-5f;
                playerNotSavedData.speed = speed;
                playerNotSavedData.currentPoints++;
                statsOnScreen[7].text = "SPEED: " + playerNotSavedData.speed.ToString()+ "/ (Real speed) : " + (playerNotSavedData.speed - playerNotSavedData.weight/200);;
                return true;
            case 8:
                if (playerNotSavedData.boostSpeed<40)
                {
                    print("you cant reduce the boost speed");
                } 
                float boost=playerNotSavedData.boostSpeed-10f;
                playerNotSavedData.boostSpeed = boost;
                statsOnScreen[9].text = "BOOST SPEED: " + playerNotSavedData.boostSpeed.ToString()+ "/ (Real BOOST) : " + (playerNotSavedData.boostSpeed - playerNotSavedData.weight/200);
                playerNotSavedData.currentPoints++;
                return true;
        }
        return false;
    }
    public void ConfirmButton()
    {
        playerScriptableObject.health = playerNotSavedData.health;
        playerScriptableObject.suppliesOnCharge = playerNotSavedData.suppliesOnCharge;
        playerScriptableObject.weight = playerNotSavedData.weight;
        playerScriptableObject.turnForce = playerNotSavedData.turnForce;
        playerScriptableObject.heightSpeedForce = playerNotSavedData.heightSpeedForce;
        playerScriptableObject.boostTime = playerNotSavedData.boostTime;
        playerScriptableObject.boostRefreshTime = playerNotSavedData.boostRefreshTime;
        playerScriptableObject.cadence = playerNotSavedData.cadence;
        playerScriptableObject.damageDone = playerNotSavedData.damageDone;
        playerScriptableObject.currentPoints = playerNotSavedData.currentPoints;
        //speed needs to be set in the buffer of the speed
        //invokes
       
        OnConfirmButtonEvent?.Invoke();
        RemoveCanvas();


    }
    //Reset the values that have been modified
    public void ResetStats()
    {
        playerNotSavedData = new PlayerDataBeforeConfirm(playerScriptableObject.health, playerScriptableObject.suppliesOnCharge, playerScriptableObject.speed, playerScriptableObject.weight,
    playerScriptableObject.turnForce, playerScriptableObject.heightSpeedForce, playerScriptableObject.boostTime, playerScriptableObject.boostRefreshTime,
    playerScriptableObject.cadence, playerScriptableObject.damageDone, playerScriptableObject.currentPoints,playerScriptableObject.boostSpeed);
        InitializateStats();
    }
    public struct PlayerDataBeforeConfirm
    {
        public int health;
        public int suppliesOnCharge;
        public float speed;
        public float weight;
        public float turnForce;
        public float heightSpeedForce;
        public float boostTime;
        public float boostRefreshTime;
        public float cadence;
        public float damageDone;
        public int currentPoints;
        public float boostSpeed;
        public PlayerDataBeforeConfirm(int health, int suppliesOnCharge,float speed, float weight, float turnForce, float heightSpeedForce, float boostTime, float boostRefreshTime, float cadence, float damageDone, int currentPoints,float boostSpeed)
        {
            this.health = health;
            this.suppliesOnCharge = suppliesOnCharge;
            this.speed = speed;
            this.weight = weight;
            this.turnForce = turnForce;
            this.heightSpeedForce = heightSpeedForce;
            this.boostTime = boostTime;
            this.boostRefreshTime = boostRefreshTime;
            this.cadence = cadence;
            this.damageDone = damageDone;
            this.currentPoints = currentPoints;
            this.boostSpeed = boostSpeed;
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerStatsController>(out PlayerStatsController player))
        {
            UI.SetActive(false);
            GameManager.instance.GameStateUpdater(GameManager.GameState.baseZone);
            playerTransform = player.transform;
            //here is suppose to be the setter of the scriptable data that will be handled
            playerScriptableObject = player.playerScriptableData;
            player.MovementeStateUpdater(PlayerStatsController.MovementState.normalMode);
            //set the not saved data
            SetDataPlayerScriptableObject();
            //upgradable, because it could be better handled;
            playerNotSavedData.speed=player.playerScriptableBuffer.speed;
            playerNotSavedData.boostSpeed = player.playerScriptableBuffer.boostSpeed;
            Debug.Log("enetered");
            //initializate the values of the canvas
            InitializateStats();
            baseUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerStatsController>(out PlayerStatsController player))
        {
            //player data is saved again in the main controller of the data
            //Refresh the values of the scriptable object
            player.playerScriptableBuffer.speed = playerNotSavedData.speed;
            player.playerScriptableBuffer.boostSpeed = playerNotSavedData.boostSpeed;
            PlayerStatsController.OnPlayerMovementValuesChange?.Invoke();
            PlayerStatsController.OnHitEvent?.Invoke();
            UI.SetActive(true);


        }
    }
    void RemoveCanvas()
    {
        GameManager.instance.GameStateUpdater(GameManager.GameState.onGame);

        baseUI.SetActive(false);
        

    }

}
