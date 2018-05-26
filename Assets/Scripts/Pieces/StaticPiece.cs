using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPiece : NonMovablePiece {

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        pieceType = PieceType.statice;
    }
}
