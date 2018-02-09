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

	[HideInInspector]
	public BoardManager boardManager;
	[HideInInspector]
	public EventManager eventManager;

    [HideInInspector]
    public enum GameState
    {
        Editor,
        Menu,
        InGame
    };

    [HideInInspector]
    public GameState gameState;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this);
		gameManager = this;
        gameState = GameState.InGame;
        eventManager = new EventManager(this);
    }

    void Start()
    {
		cameraManager = CameraManager.cameraManager;
        boardManager = new BoardManager();
        boardManager.CreateBoard(5);
        //boardManager.CreateRandomLevel();
    }
}
