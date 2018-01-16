﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Level  {

	public int size;
	public List <PieceInfo> PieceList = new List<PieceInfo>();
	public Vector2 GoalPos;

	public Level(int size)
	{
		this.size = size;
	}

	public void SetGoalPosition(Vector2 goalPos)
	{
		this.GoalPos = goalPos;
	}

	public void AddPieceElement(PieceInfo pieceElement)
	{
		if(!PieceList.Contains(pieceElement))
			PieceList.Add (pieceElement);
	}
}