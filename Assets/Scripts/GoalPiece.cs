using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPiece : MovablePiece {

    // Use this for initialization
    void Start()
    {
        base.Start();
        boardManager.SetGoalPiece(this);
        pieceType = PieceType.goal;
    }

    public void UpdateGoalPiece()
    {
        boardManager.SetGoalPiecePos(desiredPosition);
    }

    public override void UpdatePosition(Vector2 newPos)
    {
        base.UpdatePosition(newPos);
        gameManager.GetBoardManager().SetGoalPiecePos(newPos);
    }
}
