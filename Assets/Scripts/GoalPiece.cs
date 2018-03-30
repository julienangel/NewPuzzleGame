using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPiece : MovablePiece {

    // Use this for initialization
    void Start()
    {
        base.Start();
        boardManager.SetGoalPiece(this);
    }

    public void UpdateGoalPiece()
    {
        boardManager.SetGoalPiecePos(desiredPosition);
    }
}
