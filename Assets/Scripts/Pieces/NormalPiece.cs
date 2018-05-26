using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPiece : MovablePiece {

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        pieceType = PieceType.normal;
    }
}
