using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    GameManager gm;

	private Vector2 desiredPosition;

	public enum PieceType{
		Playable,
		Static, 
		MainPiece,
		Goal
	};

	public PieceType pieceType;

	// Use this for initialization
	void Start () {
        gm = GameManager.gameManager;
	}

	public void Move()
	{
		transform.localPosition = desiredPosition;
	}

	public void SetDesiredPosition(Vector2 newPos)
	{
		desiredPosition = newPos;
	}
}
