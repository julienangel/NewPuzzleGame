using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
	//Accessible to anywhere
	public static GameManager gameManager;

	[HideInInspector]
	public CameraManager cameraManager;

	public InputHandler inputHandler;

	public LevelEditorManager levelEditorManager;
	
	private BoardManager boardManager;

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
	void Awake () {
		DontDestroyOnLoad (this);
		gameManager = this;
        gameState = GameState.InGame;
        boardManager = new BoardManager();
    }

    void Start()
    {
		cameraManager = CameraManager.cameraManager;
        gameState = GameState.InGame;
        boardManager.CreateBoard(6);
        //boardManager.CreateRandomLevel();
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
}
