using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    public static Action OnScoreChangeHud;
    //first window
    [Header("Game")]
    public TextMeshProUGUI counter;
    public Slider pakageDropped;
    public Image bgBeforeStarting;
    //second window
    public Event AddingNewEventArgs;
    [Header("Render")]
    public RawImage pixelArt;
    //booster

    //Pause
    [Header("Pause")]
    public GameObject pause;
    public GameObject controls;
    
    public bool isPaused;
    public Animator pauseAnim;
    public AnimationClip[] animations;
    public TextMeshProUGUI distance;

    [Header("HP and Score")] 
    public TextMeshProUGUI score;
    public TextMeshProUGUI healthHUD;


    [Header("End Screen")]
    public GameObject EndHUD;
    public int indexStats=1;
    public GameObject[] stats;
    public PlayerStatsController player;

    [Header("Tutorial")]
    public GameObject tutorialObj;
    public GameObject[] tutorialDataObj; 
    public int index = 0;

    [Header("Out off bombs text")]
    public TextMeshProUGUI onSpawnText;

    private void Awake()
    {
        indexStats =1;
    }
    void Start()
    {
        isPaused = false;
        UpdateHP();
        UpdateScore();
        PlayerStatsController.OnHitEvent += UpdateHP;
        OnScoreChangeHud += UpdateScore;
        stats[0].GetComponentInChildren<TextMeshProUGUI>().text = "TOTAL POINTS: " + player.totalPoints.ToString();
        stats[1].GetComponentInChildren<TextMeshProUGUI>().text = "SUPPLIES DEPLOYED: " + player.totalPoints.ToString();
        stats[2].GetComponentInChildren<TextMeshProUGUI>().text = "TOTAL ENEMIES KILLED: " + player.enemiesKilled.ToString();
        stats[3].GetComponentInChildren<TextMeshProUGUI>().text = "DISTANCE TRAVELLED: " + player.distanceTravelled.ToString();
    }
    
    void Update()
    {
        if (GameManager.instance.currentState==GameManager.GameState.intro)
        {
            HandleTutorialObj();
        }
        if (PlayerStatsController.onHealthChange?.Invoke()<=0)
        {
            GameManager.instance.GameStateUpdater(GameManager.GameState.ended);

            EndHUD.SetActive(true);
            stats[0].SetActive(true);
            stats[0].GetComponentInChildren<TextMeshProUGUI>().text = "TOTAL POINTS: " + player.totalPoints.ToString();
            stats[1].GetComponentInChildren<TextMeshProUGUI>().text = "BOMBS DEPLOYED: " + player.suppliesDroppedOnSpot.ToString();
            stats[2].GetComponentInChildren<TextMeshProUGUI>().text = "TOTAL ENEMIES KILLED: " + player.enemiesKilled.ToString();
            stats[3].GetComponentInChildren<TextMeshProUGUI>().text = "DISTANCE TRAVELLED: " + player.distanceTravelled.ToString();

            Time.timeScale = 0.0f;
        }
        if (GameManager.instance.currentState==GameManager.GameState.onGame) {
            distance.text = "Distance: " + PlayerStatsController.dstToTarget.ToString("0,0");
            //pause
            HandlePause();
            
        }
        counter.text =((int)(GameManager.instance.timeToDeliver - GameManager.instance.currentRoundTime)).ToString();
        pakageDropped.value = GameManager.instance.supplyDropped;
    }
    public void LoadNextText()
    {
        if (indexStats>=stats.Length)
        {
            return;
        }
  
        stats[indexStats].gameObject.SetActive(true);
        indexStats++;
        
    }
    
    public void BombText(bool value)
    {
        onSpawnText.gameObject.SetActive(value);
    }
    public void DeactivateBombText()
    {
        onSpawnText.gameObject.SetActive(false);
    }
    public void UpdateHP()
    {
        //get the health data
        healthHUD.text ="Spaceship state: "+ PlayerStatsController.onHealthChange?.Invoke().ToString()+"%";

    }

    public void UpdateScore()
    {
        //get the health data
        score.text = "Points: " + PlayerStatsController.onScoreChange?.Invoke().ToString();
    }
    public void HandleTutorialObj()
    {
        if (index == tutorialDataObj.Length)
        {
            GameManager.instance.GameStateUpdater(GameManager.GameState.onGame);
            tutorialObj.SetActive(false);
            return;

        }
            tutorialObj.SetActive(true);
            tutorialDataObj[index].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Return) )
            {
                index++;
                if(index < tutorialDataObj.Length){
                tutorialDataObj[index - 1].SetActive(false);
                tutorialDataObj[index].SetActive(true);
                }

            }


    }
    public void SetPixelArt()
    {
        if (pixelArt.gameObject.activeSelf)
        {
            pixelArt.gameObject.SetActive(false);
        }
        else
        {
            pixelArt.gameObject.SetActive(true);
        }
        
    }
    
    
    public void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&isPaused==false)
        {
            PauseGame();
            return;
        }
        if(Input.GetKeyDown(KeyCode.Escape)&&isPaused)
        {
            ResumeGame();
            controls.SetActive(false);
        }
    }
    public void DisplayControls(bool value)
    {
        controls.SetActive(value);
    }
    public void PauseGame()
    {
        pause.SetActive(true);
        isPaused = true;
        pauseAnim.SetBool("isPaused",true);

        Time.timeScale = 0;
        
    }
    public void PlayAgain()
    {
        
        SceneManager.LoadScene("Game",LoadSceneMode.Single);
        SceneManager.LoadScene("Lights", LoadSceneMode.Additive);
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        GameManager.instance.isNewGame = true;
    }
    public void ResumeGame()
    {
        
        Time.timeScale = 1;
        isPaused = false;
        pauseAnim.SetBool("isPaused",false);

        
    }

    public void QuitGame()
    {
        //AsyncOperation operation = SceneManager.LoadSceneAsync("Menu",LoadSceneMode.Single);
        //if (operation.progress>=0.98f)
        //{
        //    operation.allowSceneActivation = true;
        //}
        Application.Quit();
    }

    public void QuitPauseAnim()
    {
        pause.SetActive(false);
    }

    public void StartTutorial()
    {


        
    }


}
