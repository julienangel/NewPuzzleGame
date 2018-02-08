using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Level  {

	public int size;
	public List <PieceInfo> PieceList = new List<PieceInfo>();

	public Level(int size)
	{
		this.size = size;
	}

	public void AddPieceElement(PieceInfo pieceElement)
	{
		if(!PieceList.Contains(pieceElement))
			PieceList.Add (pieceElement);
	}
}
