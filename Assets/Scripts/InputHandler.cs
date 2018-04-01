using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	GameManager gm;
    BoardManager boardManager;
    LevelEditorManager lvlEditorManager;

	[HideInInspector]
	public enum MoveDirection
	{
		Up,
		left,
        down,
		right
	};

    [HideInInspector]
	public static MoveDirection moveDirection;

	// Use this for initialization
	void Start () {
		gm = GameManager.gameManager;
        boardManager = gm.GetBoardManager();
        lvlEditorManager = gm.GetLevelEditorManager();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			if (gm.GetGameState() == GameManager.GameState.Editor)
			{
				lvlEditorManager.EditOrPlacePiece (ConvertScreenToWorldPosition (Input.mousePosition));
			}
		}

		if(gm.GetGameState() == GameManager.GameState.InGame)
		{
			if(Input.GetKeyDown(KeyCode.S))
			{
				moveDirection = MoveDirection.down;
				StartCoroutine(gm.GetBoardManager().MovePieces (moveDirection));
			}

			else if(Input.GetKeyDown(KeyCode.W))
			{
				moveDirection = MoveDirection.Up;
                StartCoroutine(gm.GetBoardManager().MovePieces (moveDirection));
			}

			else if(Input.GetKeyDown(KeyCode.A))
			{
				moveDirection = MoveDirection.left;
                StartCoroutine(gm.GetBoardManager().MovePieces (moveDirection));
			}

			else if(Input.GetKeyDown(KeyCode.D))
			{
				moveDirection = MoveDirection.right;
                StartCoroutine(gm.GetBoardManager().MovePieces (moveDirection));
			}
		}
	}

	public Vector2 ConvertScreenToWorldPosition(Vector3 mousePosition)
	{
		Vector3 v3 = Camera.main.ScreenToWorldPoint (mousePosition);
		return new Vector2 (v3.x, v3.y);
	}
}
