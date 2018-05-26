using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece : Piece {

    const float PIECE_TIME_VELOCITY = 0.15f;

    protected Vector2 desiredPosition;
    protected Vector2 startPosition;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    public IEnumerator Move()
    {
        Vector2 current = transform.localPosition;
        if (current == desiredPosition)
        {
            boardManager.EndedPiecesMovement();
            yield break;
        }

        float t = 0f;
        if (gameManager.GetGameState() != EditorManager.GameState.Solving)
        {
            while (t < 1)
            {
                t += Time.deltaTime / PIECE_TIME_VELOCITY;
                transform.localPosition = Vector2.Lerp(current, desiredPosition, t);
                yield return null;
            }
        }
        else
            transform.localPosition = desiredPosition;

        boardManager.EndedPiecesMovement();
        UpdatePosition(desiredPosition);
        yield return null;
    }

    public void SetDesiredPosition(Vector2 newPos)
    {
        desiredPosition = new Vector2(newPos.x, newPos.y);
    }

    public virtual void UpdatePosition(Vector2 newPos)
    {
        this.position = newPos;
    }
}
