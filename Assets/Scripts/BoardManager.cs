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

		if (pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] != null) {
			if (Input.GetKey (KeyCode.LeftShift)) {
				GameObject.Destroy (pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y].gameObject);
				pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] = null;
			}
		} else {
			if (Input.GetKey (KeyCode.A)) {
				pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] = InstantiateGameObject (movablePiece, vecRoundToInt);
			} else if (Input.GetKey (KeyCode.S)) {
				pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] = InstantiateGameObject (staticPiece, vecRoundToInt);
			} else if (Input.GetKey (KeyCode.D)) {
				pieceBoard [(int)vecRoundToInt.x, (int)vecRoundToInt.y] = InstantiateGameObject (mainPiece, vecRoundToInt);
			}
		}
	}

	public Piece InstantiateGameObject (GameObject objectToInstantiate, Vector2 pos)
	{
		return GameObject.Instantiate (objectToInstantiate, new Vector2 ((int)pos.x, (int)pos.y), Quaternion.identity).GetComponent<Piece> ();
	}

	public void MovePieces (InputHandler.MoveDirection md)
	{
		List<Piece> piecesToMove = new List<Piece> ();
		switch (md) {
		// starts ignoring the first line
		case InputHandler.MoveDirection.down:
			for (int i = 0; i < pieceBoard.GetLength (0); i++) {
				for (int j = 0; j < pieceBoard.GetLength (1); j++) {
					for (int k = j - 1; k >= 0; k--) {
						if (pieceBoard [i, j] != null && pieceBoard [i, k] == null) {
							if (pieceBoard [i, j].pieceType != Piece.PieceType.Static) {
								pieceBoard [i, k] = pieceBoard [i, j];
								pieceBoard [i, j] = null;
								pieceBoard [i, k].SetDesiredPosition (new Vector2 (i, k));
								if (!piecesToMove.Contains (pieceBoard [i, k]))
									piecesToMove.Add (pieceBoard [i, k]);
								j--;
							}
						} else
							break;
					}
				}
			}
			break;

		case InputHandler.MoveDirection.Up:
			for (int i = 0; i < pieceBoard.GetLength (0); i++) {
				for (int j = pieceBoard.GetLength (1) - 2; j >= 0; j--) {
					for (int k = j + 1; k <= pieceBoard.GetLength (1) - 1; k++) {
						if (pieceBoard [i, j] != null && pieceBoard [i, k] == null) {
							if (pieceBoard [i, j].pieceType != Piece.PieceType.Static) {
								pieceBoard [i, k] = pieceBoard [i, j];
								pieceBoard [i, j] = null;
								pieceBoard [i, k].SetDesiredPosition (new Vector2 (i, k));
								if (!piecesToMove.Contains (pieceBoard [i, k]))
									piecesToMove.Add (pieceBoard [i, k]);
								j++;
							}
						} else
							break;
					}
				}
			}
			break;

		case InputHandler.MoveDirection.left:
			for (int i = 1; i < pieceBoard.GetLength (0); i++) {
				for (int j = 0; j < pieceBoard.GetLength (1); j++) {
					for (int k = i - 1; k >= 0; k--) {
						if (pieceBoard [i, j] != null && pieceBoard [k, j] == null) {
							if (pieceBoard [i, j].pieceType != Piece.PieceType.Static) {
								pieceBoard [k, j] = pieceBoard [i, j];
								pieceBoard [i, j] = null;
								pieceBoard [k, j].SetDesiredPosition (new Vector2 (k, j));
								if (!piecesToMove.Contains (pieceBoard [k, j]))
									piecesToMove.Add (pieceBoard [k, j]);
								i--;
							}
						} else
							break;
					}
				}
			}
			break;

		case InputHandler.MoveDirection.right:
			for (int i = pieceBoard.GetLength (0) - 2; i >= 0; i--) {
				for (int j = 0; j < pieceBoard.GetLength (1); j++) {
					for (int k = i + 1; k <= pieceBoard.GetLength (0) - 1; k++) {
						if (pieceBoard [i, j] != null && pieceBoard [k, j] == null) {
							if (pieceBoard [i, j].pieceType != Piece.PieceType.Static) {
								pieceBoard [k, j] = pieceBoard [i, j];
								pieceBoard [i, j] = null;
								pieceBoard [k, j].SetDesiredPosition (new Vector2 (k, j));
								if (!piecesToMove.Contains (pieceBoard [k, j]))
									piecesToMove.Add (pieceBoard [k, j]);
								i++;
							}
						} else
							break;
					}
				}
			}
			break;
		}
		int piecesToMoveCount = piecesToMove.Count;
		for (int i = 0; i < piecesToMoveCount; i++) {
			piecesToMove [i].Move ();
		}
	}
}