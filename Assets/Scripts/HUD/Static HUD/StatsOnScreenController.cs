using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;
using Microsoft.Win32.SafeHandles;

public class StatsOnScreenController : MonoBehaviour
{
    public Animator animatorRef;
    public PlayerStatsController player;
    public PlayerData playerScriptableObject;
    public TextMeshProUGUI[] stats;
    public TextMeshProUGUI buttonText;
    public Transform baseRef;
    private void Awake()
    {
        animatorRef = GetComponent<Animator>();
        playerScriptableObject = player.playerScriptableData;
        StatsOnScreen();
        animatorRef.SetBool("showed", false);

    }
    void Start()
    {
        
    }
    private void Update()
    {

            StatsOnScreen();
        
    }
    public void StatsOnScreen()
    {
        stats[0].text = "Suplies on Charge: "+playerScriptableObject.suppliesOnCharge.ToString();
        stats[1].text = "Weight: " + playerScriptableObject.weight.ToString();
        stats[2].text = "Height: " + player.GetComponent<PlayerController>().radius.ToString();
        stats[3].text = "Speed: "+ player.GetComponent<PlayerController>().forwardSpeed.ToString()+" KM/H";
        stats[4].text = "Current Points: " + playerScriptableObject.currentPoints.ToString();
        stats[5].text = "Distance To Base: " + (baseRef.position - player.transform.position).magnitude.ToString(); 

    }
    public void ToggleOn()
    {
        if (!animatorRef.GetBool("showed"))
        {
            animatorRef.SetBool("showed", true);
            buttonText.text = ">";
        }
        else
        {
            animatorRef.SetBool("showed", false);
            buttonText.text = "<";
        }
        


    }



}
