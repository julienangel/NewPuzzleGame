using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    //public


    //protected
    protected GameManager gameManager;
    protected BoardManager boardManager;
    protected Vector2 position;

    //private


    //Getters and setters
    public Vector2 Position { get { return position; } }

    // Use this for initialization
    public virtual void Start()
    {
        gameManager = GameManager.gameManager;
        position = transform.localPosition;
        boardManager = gameManager.GetBoardManager();

        boardManager.SetElementOnBoard((int)Position.x, (int)position.y, this);
    }
}
