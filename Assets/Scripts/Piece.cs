﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	public enum PieceType{
		Playable,
		Static, 
		Goal
	};

	public PieceType pieceType;

	// Use this for initialization
	void Start () {
		
	}
}