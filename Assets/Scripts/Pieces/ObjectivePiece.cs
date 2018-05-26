using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePiece : NonMovablePiece {

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        boardManager.SetObjectivePiece(this);
        pieceType = PieceType.objective;
    }
}
