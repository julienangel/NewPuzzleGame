using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    GameManager gm;

    [HideInInspector]
    public static float PIECE_TIME_VELOCITY = 0.1f;

	private Vector2 desiredPosition;
    private Vector2 startPosition;

	public enum PieceType{
		Playable,
		Static,
		MainPiece,
		Goal
	};

	public PieceType pieceType;

	// Use this for initialization
	void Start () {
        gm = GameManager.gameManager;

        startPosition = transform.localPosition;

        if(pieceType == PieceType.Goal)
        {
            gm.boardManager.goalPosBoard = transform.localPosition;
        }
        else if(pieceType == PieceType.MainPiece)
        {
            gm.boardManager.mainPiecePosBoard = transform.localPosition;
        }
	}

	public IEnumerator Move()
	{
        //transform.localPosition = desiredPosition;
        Vector2 current = transform.localPosition;
        if (current == desiredPosition)
        {
            gm.boardManager.EndedPiecesMovement();
            yield break;
        }
            
        //float t = 0f;
        //while(t < 1)
        //{
            //t += Time.deltaTime / PIECE_TIME_VELOCITY;
            //transform.localPosition = Vector2.Lerp(current, desiredPosition, t);
            transform.localPosition = desiredPosition;
            yield return null;
        //}

        if (pieceType == PieceType.Goal)
        {
            gm.boardManager.goalPosBoard = transform.localPosition;
        }
        else if (pieceType == PieceType.MainPiece)
        {
            gm.boardManager.mainPiecePosBoard = transform.localPosition;
        }

        gm.boardManager.EndedPiecesMovement();
	}

	public void SetDesiredPosition(Vector2 newPos)
	{
		desiredPosition = newPos;
	}

    public void BackToOriginalPosition()
    {
        desiredPosition = startPosition;
        StartCoroutine(Move());
    }
}
