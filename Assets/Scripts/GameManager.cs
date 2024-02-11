using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameState { TitleState, CreditsState, GameplayState, GameOverState, VictoryState }; //Game States

public class GameStateChangedEvent : UnityEvent<GameState, GameState> { }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGamePaused = false;

    public LayerMask yinLayerMask;
    public LayerMask yangLayerMask;

    public GameState currentGameState;
    private GameState previousGameState;
    public GameStateChangedEvent OnGameStateChanged = new GameStateChangedEvent();
    //public GameObject GameOverPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // On load, the game state is the title state
        // game state is set to gameplay state for debugging resons rn
        ChangeGameState(GameState.TitleState);
        //GameOverPanel.gameObject.SetActive(false);

    }

    public void StartGame()
    {
        ChangeGameState(GameState.GameplayState);
        //GameOverPanel.gameObject.SetActive(false);
    }

    void Update()
    {
    
    }

    public void PolaritySwitched()
    {
        Debug.Log("Polarity switched!");
    }
    
    public void ChangeGameState(GameState state)
    {
        previousGameState = currentGameState;
        currentGameState = state;
        OnGameStateChanged.Invoke(previousGameState, currentGameState);
    }

    private void OnEnable()
    {
        // Subscribe to universal controller events
        UniversalController.OnQuitGame += QuitGame;
        UniversalController.OnTogglePause += TogglePause;
    }

    private void OnDisable()
    {
        // Unsubscribe from universal controller events
        UniversalController.OnQuitGame -= QuitGame;
        UniversalController.OnTogglePause -= TogglePause;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game");
        Application.Quit();
    }

    public void GameOver()
    {
        TogglePause();
        ChangeGameState(GameState.GameOverState);
        Debug.Log("game over!!");
        //GameOverPanel.gameObject.SetActive(true);



    }

    private void TogglePause()
    {
        //toggle the game pause bool
        isGamePaused = !isGamePaused;
        // TO DO: add UI to show pause
        if (isGamePaused)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }

        Debug.Log("Game " + (isGamePaused ? "Paused" : "Resumed"));
    }
}
