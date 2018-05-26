using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	EditorManager editorManager;
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
		editorManager = EditorManager.Instance;
        boardManager = editorManager.GetBoardManager();
        lvlEditorManager = editorManager.GetLevelEditorManager();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			if (editorManager.GetGameState() == EditorManager.GameState.Editor)
			{
				lvlEditorManager.EditOrPlacePiece (ConvertScreenToWorldPosition (Input.mousePosition));
			}
		}

		if(editorManager.GetGameState() == EditorManager.GameState.InGame)
		{
			if(Input.GetKeyDown(KeyCode.S))
			{
				moveDirection = MoveDirection.down;
				StartCoroutine(editorManager.GetBoardManager().MovePieces (moveDirection));
			}

			else if(Input.GetKeyDown(KeyCode.W))
			{
				moveDirection = MoveDirection.Up;
                StartCoroutine(editorManager.GetBoardManager().MovePieces (moveDirection));
			}

			else if(Input.GetKeyDown(KeyCode.A))
			{
				moveDirection = MoveDirection.left;
                StartCoroutine(editorManager.GetBoardManager().MovePieces (moveDirection));
			}

			else if(Input.GetKeyDown(KeyCode.D))
			{
				moveDirection = MoveDirection.right;
                StartCoroutine(editorManager.GetBoardManager().MovePieces (moveDirection));
			}
		}
	}

	public Vector2 ConvertScreenToWorldPosition(Vector3 mousePosition)
	{
		Vector3 v3 = Camera.main.ScreenToWorldPoint (mousePosition);
		return new Vector2 (v3.x, v3.y);
	}
}
