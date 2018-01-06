using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    GameManager gm;

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
}
