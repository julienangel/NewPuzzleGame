using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Level  {

	public int size;
	public List <PieceInfo> PieceList = new List<PieceInfo>();
    public List <InputHandler.MoveDirection> directionListSolution = new List<InputHandler.MoveDirection>();

	public Level(int size)
	{
		this.size = size;
	}

	public void AddPieceElement(PieceInfo pieceElement)
	{
		if(!PieceList.Contains(pieceElement))
			PieceList.Add (pieceElement);
	}

    public void SaveSolution(List<InputHandler.MoveDirection> directionsSolution)
    {
        directionListSolution = new List<InputHandler.MoveDirection>(directionsSolution);
    }
}
