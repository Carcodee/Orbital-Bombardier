using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("player")]
    public float maxTimeShootPlayer;
    public float maxTimeBooster;

    [Header("Save System")]
    public bool isNewGame;
    [Header("enemys")]
    public float maxTimeShootEnemy;

    [Header("Game")]
    public float RoundTime;
    public float timeToDeliver;

    public float currentRoundTime;
    public float supplyDropped = 0;
    public float rateOfPlanetDestruction;
    //State
    public GameState currentState;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
        // Con esto indicamos que no queremos que este gameobject
        // desaparezca del campo de escena. 
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        GameStateUpdater(GameState.menu);
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
        GameStateUpdater(currentState);
        if (supplyDropped>RoundTime)
        {
            GameStateUpdater(GameState.ended);
        }
    }

    public void GameStateUpdater(GameState newState) {
        currentState = newState;
        switch (newState)
        {
            case GameState.menu:
                break;
            case GameState.baseZone:
                break;
            case GameState.intro:
                break;
            case GameState.onGame:
                currentRoundTime += Time.deltaTime;
                //this means the current value of the bar of the planet destruction!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                supplyDropped += rateOfPlanetDestruction;
                if (supplyDropped>=0.98)
                {
                    GameStateUpdater(GameState.ended);
                }
                break;
            case GameState.endedLose:
                break;
            case GameState.ended:
                break;
                
        }
    }
    public void SetGameToContinue()
    {
        instance.isNewGame = false;
        GameStateUpdater(GameState.onGame);
    }
    public void SetNewGame()
    {
        instance.isNewGame = true;
        GameStateUpdater(GameState.intro);
    }

    public enum GameState {
        menu,
        baseZone,
        intro,
        onGame,
        endedLose,
        ended
    }

}
