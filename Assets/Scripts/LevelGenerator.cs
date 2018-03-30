using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator {

	public LevelGenerator(int size)
	{
		GenerateNewLevel (size);
	}

	public Level GenerateNewLevel(int size)
	{
		Level level = new Level (size);

		//PieceInfo mainPiece = new PieceInfo (new Vector2 (0, 0), Piece.PieceType.MainPiece);

		return level;
	}
}
