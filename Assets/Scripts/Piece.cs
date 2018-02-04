using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    GameManager gm;

    const float PIECE_TIME_VELOCITY = 0.25f;

	private Vector2 desiredPosition;

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

        if(pieceType == PieceType.Goal)
        {
            gm.boardManager.goalPosBoard = transform.localPosition;
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
            
        float t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / PIECE_TIME_VELOCITY;
            transform.localPosition = Vector2.Lerp(current, desiredPosition, t);
            yield return null;
        }

        if (pieceType == PieceType.MainPiece)
        {
            gm.boardManager.mainPiecePosBoard = transform.localPosition;
        }

        gm.boardManager.EndedPiecesMovement();
	}

	public void SetDesiredPosition(Vector2 newPos)
	{
		desiredPosition = newPos;
	}
}
