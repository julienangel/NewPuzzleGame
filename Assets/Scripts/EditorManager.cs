using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorManager : MonoBehaviour
{
    private static EditorManager instance;
    public static EditorManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EditorManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    InputHandler inputHandler;

    [SerializeField]
    private LevelEditorManager levelEditorManager;

    private BoardManager boardManager;

    [SerializeField]
    Text numberPlays;

    private int nPlays = 0;

    [HideInInspector]
    public enum GameState
    {
        Editor,
        Menu,
        InGame,
        Solving
    };

    private GameState gameState;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        gameState = GameState.InGame;
        boardManager = new BoardManager();
    }

    void Start()
    {
        gameState = GameState.InGame;
        boardManager.CreateBoard(6);
    }

    public BoardManager GetBoardManager()
    {
        return boardManager;
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState(GameState newGameState)
    {
        gameState = newGameState;
    }

    public LevelEditorManager GetLevelEditorManager()
    {
        return levelEditorManager;
    }

    public void IncreasePlay()
    {
        nPlays++;
        numberPlays.text = nPlays.ToString();
    }

    public void ResetCount()
    {
        nPlays = -1;
        IncreasePlay();
    }
}
