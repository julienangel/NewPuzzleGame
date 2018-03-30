using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePiece : NonMovablePiece {

    // Use this for initialization
    void Start()
    {
        base.Start();
        boardManager.SetObjectivePiece(this);
    }
}
