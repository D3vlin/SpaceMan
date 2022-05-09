using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    public GameState currentGameState = GameState.menu;
    public int collectableObject = 0;

    public AudioSource backgroundMusic;
    public AudioSource gameOverMusic;

    private PlayerController playerController;

    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && currentGameState != GameState.inGame)
        {
            StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.P) && currentGameState != GameState.menu)
        {
            BackToMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && currentGameState != GameState.gameOver)
        {
            GameOver();
        }

        if (currentGameState == GameState.menu)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void StartGame()
    {
        backgroundMusic.Play();
        SetGameState(GameState.inGame);
    }

    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }

    public void GameOver()
    {
        backgroundMusic.Stop();
        gameOverMusic.Play();
        SetGameState(GameState.gameOver);
    }

    private void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {
            MenuManager.sharedInstance.ShowMainMenu();
        }
        else if (newGameState == GameState.inGame)
        {
            LevelManager.sharedInstance.RemoveAllLevelBlocks();
            LevelManager.sharedInstance.GenerateInicialBlocks();
            playerController.StarGame();
            MenuManager.sharedInstance.HideMainMenu();
        }
        else if (newGameState == GameState.gameOver)
        {
            MenuManager.sharedInstance.ShowGameOver();
        }

        this.currentGameState = newGameState;
    }

    public void CollectObject(Collectable collectable)
    {
        collectableObject += collectable.value;
    }
}
