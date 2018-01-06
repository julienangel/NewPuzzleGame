using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager  {

	public GameObject[,] pieceBoard;
	public GameObject[,] backgroundBoard;

    public GameObject movablePiece = null;
    public GameObject mainPiece = null;
    public GameObject staticPieces = null;
	public GameObject backgroundPiece = null;

	public BoardManager()
    {
        //this.mono = mono;
        movablePiece = Resources.Load<GameObject>("Prefabs/GamePieces/Piece");
		backgroundPiece = Resources.Load<GameObject> ("Prefabs/GamePieces/Background");
	}

    public void CreateBoard(int size)
    {
		PlaceBackgroundPieces (size);
		PlacePieces (size);
        
		CameraManager.cameraManager.SetPositionAndOrtographicSize (size);
    }

	public void PlacePieces(int size)
    {
		pieceBoard = new GameObject[size, size];

        for (int i = 0; i < pieceBoard.GetLength(0); i++)
        {
            for (int j = 0; j < pieceBoard.GetLength(1); j++)
            {
                pieceBoard[i, j] = GameObject.Instantiate(movablePiece, new Vector2(i, j), Quaternion.identity);
            }
        }
    }

	public void PlaceBackgroundPieces(int size)
	{
		backgroundBoard = new GameObject[size, size];

		for (int i = 0; i < backgroundBoard.GetLength(0); i++) 
		{
			for (int j = 0; j < backgroundBoard.GetLength(1); j++) 
			{
				backgroundBoard [i, j] = GameObject.Instantiate (backgroundPiece, new Vector2 (i, j), Quaternion.identity);
			}
		}
	}
}
