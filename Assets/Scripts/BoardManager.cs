using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager
{

	const float detectionOffset = -0.5f;

	public Piece[,] pieceBoard;
	public GameObject[,] backgroundBoard;

	public GameObject movablePiece = null;
	public GameObject mainPiece = null;
	public GameObject staticPiece = null;
	public GameObject backgroundPiece = null;

	public BoardManager ()
	{
		//this.mono = mono;
		movablePiece = Resources.Load<GameObject> ("Prefabs/GamePieces/Piece");
		backgroundPiece = Resources.Load<GameObject> ("Prefabs/GamePieces/Background");
		staticPiece = Resources.Load<GameObject> ("Prefabs/GamePieces/StaticPiece");
		mainPiece = Resources.Load<GameObject> ("Prefabs/GamePieces/MainPiece");
	}

	public void CreateBoard (int size)
	{
		PlaceBackgroundPieces (size);
		PlacePieces (size);
        
		CameraManager.cameraManager.SetPositionAndOrtographicSize (size);
	}

	public void PlacePieces (int size)
	{
		pieceBoard = new Piece[size, size];

		for (int i = 0; i < pieceBoard.GetLength (0); i++) {
			for (int j = 0; j < pieceBoard.GetLength (1); j++) {
				pieceBoard [i, j] = null;
			}
		}
	}

	public void PlaceBackgroundPieces (int size)
	{
		backgroundBoard = new GameObject[size, size];

		for (int i = 0; i < backgroundBoard.GetLength (0); i++) {
			for (int j = 0; j < backgroundBoard.GetLength (1); j++) {
				backgroundBoard [i, j] = GameObject.Instantiate (backgroundPiece, new Vector2 (i, j), Quaternion.identity);
			}
		}
	}

	public void EditOrPlacePiece (Vector2 mousePosClick)
	{
		if (mousePosClick.x < Vector2.zero.x + detectionOffset || mousePosClick.y < Vector2.zero.y + detectionOffset ||
		    mousePosClick.x > pieceBoard.GetLength (0) + detectionOffset || mousePosClick.y > pieceBoard.GetLength (1) + detectionOffset)
			return;

		Vector2 vecRoundToInt = new Vector2 (Mathf.RoundToInt (mousePosClick.x), Mathf.RoundToInt (mousePosClick.y));

		if (Input.GetKey (KeyCode.LeftShift)) {
			if (pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] != null) {
				GameObject.Destroy (pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y]);
				pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] = null;
			}
		} else if (Input.GetKey (KeyCode.A)) {
			if (pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] == null) {
				pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] = GameObject.Instantiate (movablePiece, new Vector2 ((int)vecRoundToInt.x, (int)vecRoundToInt.y), Quaternion.identity).GetComponent<Piece>();
			}
		} else if (Input.GetKey (KeyCode.S)) {
			if (pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] == null) {
				pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] = GameObject.Instantiate (staticPiece, new Vector2 ((int)vecRoundToInt.x, (int)vecRoundToInt.y), Quaternion.identity).GetComponent<Piece>();
			}
		} else if (Input.GetKey (KeyCode.D)) {
			if (pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] == null) {
				pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] = GameObject.Instantiate (mainPiece, new Vector2 ((int)vecRoundToInt.x, (int)vecRoundToInt.y), Quaternion.identity).GetComponent<Piece>();
			}
		}
	}

	public void MovePieces (InputHandler.MoveDirection md)
	{
		switch (md) {
		case InputHandler.MoveDirection.Up:
			for (int i = 0; i < pieceBoard.GetLength (0); i++) {
				for (int j = 0; j < pieceBoard.GetLength (1); j++) {
					
				}
			}
			break;
		}
	}
}
