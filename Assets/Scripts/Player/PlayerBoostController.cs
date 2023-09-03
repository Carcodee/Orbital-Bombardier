using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBoostController : MonoBehaviour
{
    public PlayerData playerScriptableData;
    public Slider slider;
    public float refreshBoostTime;
    public float BoostTime;

    public PlayerStatsController player;
    public PlayerController playerControllerRef;
    public bool isOnCd;
    [SerializeField]
    private AudioSource audioBoost;
    private void Awake()
    {

    }
    void Start()
    {
        playerScriptableData = GetComponent<PlayerStatsController>().playerScriptableData;
        PlayerStatsController.OnPlayerMovementValuesChange += InitializateValues;
        InitializateValues();
        player = GetComponent<PlayerStatsController>();
        playerControllerRef = GetComponent<PlayerController>();
        slider.value = slider.minValue;
    }

    void Update()
    {
        if (GameManager.instance.currentState == GameManager.GameState.onGame)
        {
            if (player.playerMovementState==PlayerStatsController.MovementState.normalMode)
            {
                HandleBooster();
            }
        }
    }
    public void InitializateValues()
    {
        BoostTime = playerScriptableData.boostTime;
        refreshBoostTime = playerScriptableData.boostRefreshTime;
    }
    void HandleBooster()
    {
        float boost = Input.GetAxisRaw("boost");
        //if it is on cd and the input value is false is posible to start refreshing, also if the player is not boosting we start refreshing
        if (slider.value >= slider.minValue && isOnCd == true || !playerControllerRef.ApplyBoost(boost))
        {
            //sustract the slider bar value until the smallest value
            slider.value -= refreshBoostTime;
            if (slider.value==slider.minValue)
            {
                //it is posible to start the boost again
                isOnCd = false;

            }
            return;
        }
        //if the player can boost, apply the boost to it
        if ( slider.value <= slider.maxValue && isOnCd == false && playerControllerRef.ApplyBoost(boost))
        {


            //if the value is max, set it on cd
            if (slider.value ==slider.maxValue)
            {
                
                isOnCd = true;
                //set the speed to default
                playerControllerRef.forwardSpeed = playerControllerRef.defaultSpeed();
                return;
                //se acabo el boost
            }
            //if nothing of the condition metioned before happens, apply boost.
            if (slider.value==slider.minValue&&Input.GetKeyDown(KeyCode.LeftShift))
            {
                audioBoost.Play();
            }
            playerControllerRef.ApplyBoost(boost);
            slider.value += BoostTime;
        }
    }

}
