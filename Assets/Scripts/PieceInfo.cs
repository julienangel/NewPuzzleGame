using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PieceInfo {

	public Vector2 pos;
    public Component pieceType;

	public PieceInfo(Vector2 pos, Component pieceType)
	{
		this.pos = pos;
        this.pieceType = pieceType;
	}
}
