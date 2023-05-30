using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public KingCowController player;
    public HerdManager herdManager;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject startGamePanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;

    public bool isEndless;

    private GameState gameState;
    public enum GameState
    {
        MAIN_MENU,
        INFO,
        STORY_SCREEN,
        GAME_START,
        IN_GAME,
        VICTORY,
        GAME_OVER
    }

    void Start()
    {
        SetGameState(GameState.MAIN_MENU);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.MAIN_MENU:
                ProcessMainMenu();
                break;
            case GameState.GAME_START:
                ProcessGameStart();
                break;
            case GameState.STORY_SCREEN:
                ProcessStoryScreen();
                break;
            case GameState.IN_GAME:
                ProcessInGame();
                break;
            case GameState.VICTORY:
            case GameState.GAME_OVER:
                ProcessGameOver();
                break;
            default:
                // Handles pages that are handled purely by
                // button events like the INFO page
                break;
        }
    }

    private void ProcessMainMenu()
    {
        // Intentionally left empty as actions in this state
        // are handled by button calls to other functions
    }

    public void ProcessStoryButtonOption()
    {
        isEndless = false;
        SetGameState(GameState.STORY_SCREEN);
    }

    public void ProcessEndlessButtonOption()
    {
        isEndless = true;
        SetGameState(GameState.GAME_START);
    }

    public void ProcessInfoButtonOption()
    {
        SetGameState(GameState.INFO);
    }

    public void ProcessMenuButtonOption()
    {
        SetGameState(GameState.MAIN_MENU);
    }

    private void ProcessStoryScreen()
    {
        if (Input.anyKeyDown)
        {
            SetGameState(GameState.GAME_START);
        }
    }

    private void ProcessGameStart()
    {
        if (Input.anyKeyDown)
        {
            SetGameState(GameState.IN_GAME);
        }
    }

    private void ProcessInGame()
    {
        // Game over condition
        if (!player.IsAlive || herdManager.IsHerdGone)
        {
            SetGameState(GameState.GAME_OVER);
        }
    }

    private void ProcessGameOver()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Return to the main menu
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void SetGameState(GameState state)
    {
        gameState = state;

        switch (state)
        {
            case GameState.MAIN_MENU:
                mainMenuPanel.SetActive(true);
                infoPanel.SetActive(false);
                storyPanel.SetActive(false);
                uiPanel.SetActive(false);
                startGamePanel.SetActive(false);
                victoryPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                Time.timeScale = 0;
                break;
            case GameState.INFO:
                mainMenuPanel.SetActive(false);
                infoPanel.SetActive(true);
                storyPanel.SetActive(false);
                uiPanel.SetActive(false);
                startGamePanel.SetActive(false);
                victoryPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                Time.timeScale = 0;
                break;
            case GameState.STORY_SCREEN:
                mainMenuPanel.SetActive(false);
                infoPanel.SetActive(false);
                storyPanel.SetActive(true);
                uiPanel.SetActive(false);
                startGamePanel.SetActive(false);
                victoryPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                Time.timeScale = 0;
                break;
            case GameState.GAME_START:
                mainMenuPanel.SetActive(false);
                infoPanel.SetActive(false);
                storyPanel.SetActive(false);
                uiPanel.SetActive(true);
                startGamePanel.SetActive(true);
                victoryPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                Time.timeScale = 0;
                break;
            case GameState.IN_GAME:
                mainMenuPanel.SetActive(false);
                infoPanel.SetActive(false);
                storyPanel.SetActive(false);
                uiPanel.SetActive(true);
                startGamePanel.SetActive(false);
                victoryPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                Time.timeScale = 1;
                break;
            case GameState.VICTORY:
                mainMenuPanel.SetActive(false);
                infoPanel.SetActive(false);
                storyPanel.SetActive(false);
                uiPanel.SetActive(false);
                startGamePanel.SetActive(false);
                victoryPanel.SetActive(true);
                gameOverPanel.SetActive(false);
                Time.timeScale = 0;
                break;
            case GameState.GAME_OVER:
                mainMenuPanel.SetActive(false);
                infoPanel.SetActive(false);
                storyPanel.SetActive(false);
                uiPanel.SetActive(true);
                startGamePanel.SetActive(false);
                victoryPanel.SetActive(false);
                gameOverPanel.SetActive(true);
                Time.timeScale = 0;
                break;
        }
    }
}