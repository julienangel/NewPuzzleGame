using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager
{

    const float detectionOffset = -0.5f;

    public Piece[,] pieceBoard;
    public GameObject[,] backgroundBoard;
    public Vector2 goalPosBoard;
    public Vector2 mainPiecePosBoard;

    public GameObject movablePiece = null;
    public GameObject mainPiece = null;
    public GameObject staticPiece = null;
    public GameObject backgroundPiece = null;
    public GameObject goalPiece = null;

    private bool piecesAreMoving = false;

    public BoardManager()
    {
        //this.mono = mono;
        LoadPrefabs();
    }

    void LoadPrefabs()
    {
        movablePiece = Resources.Load<GameObject>("Prefabs/GamePieces/Piece");
        backgroundPiece = Resources.Load<GameObject>("Prefabs/GamePieces/Background");
        staticPiece = Resources.Load<GameObject>("Prefabs/GamePieces/StaticPiece");
        mainPiece = Resources.Load<GameObject>("Prefabs/GamePieces/MainPiece");
        goalPiece = Resources.Load<GameObject>("Prefabs/GamePieces/Goal");
    }

    public void CreateBoard(int size)
    {
        PlaceBackgroundPieces(size);
        CreateBoardPieces(size);

        CameraManager.cameraManager.SetPositionAndOrtographicSize(size);
    }

    public void CreateBoardPieces(int size)
    {
        pieceBoard = new Piece[size, size];
        int sizeBoard = pieceBoard.GetLength(0);

        for (int i = 0; i < sizeBoard; i++)
        {
            for (int j = 0; j < sizeBoard; j++)
            {
                pieceBoard[i, j] = null;
            }
        }
    }

    public void PlaceBackgroundPieces(int size)
    {
        backgroundBoard = new GameObject[size, size];
        int sizeBackGround = backgroundBoard.GetLength(0);

        for (int i = 0; i < sizeBackGround; i++)
        {
            for (int j = 0; j < sizeBackGround; j++)
            {
                backgroundBoard[i, j] = GameObject.Instantiate(backgroundPiece, new Vector2(i, j), Quaternion.identity);
            }
        }
    }

    public void EditOrPlacePiece(Vector2 mousePosClick)
    {
        if (mousePosClick.x < Vector2.zero.x + detectionOffset || mousePosClick.y < Vector2.zero.y + detectionOffset ||
            mousePosClick.x > pieceBoard.GetLength(0) + detectionOffset || mousePosClick.y > pieceBoard.GetLength(1) + detectionOffset)
            return;

        Vector2 vecRoundToInt = new Vector2(Mathf.RoundToInt(mousePosClick.x), Mathf.RoundToInt(mousePosClick.y));

        if (pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y] != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GameObject.Destroy(pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y].gameObject);
                pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y] = null;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y] = InstantiateGameObject(movablePiece, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y] = InstantiateGameObject(staticPiece, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y] = InstantiateGameObject(mainPiece, vecRoundToInt);
            }
            else if(Input.GetKey(KeyCode.F))
            {
                pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y] = InstantiateGameObject(goalPiece, vecRoundToInt);
            }
        }
    }

    public Piece InstantiateGameObject(GameObject objectToInstantiate, Vector2 pos)
    {
        return GameObject.Instantiate(objectToInstantiate, new Vector2((int)pos.x, (int)pos.y), Quaternion.identity).GetComponent<Piece>();
    }

    public void MovePieces(InputHandler.MoveDirection md)
    {
        List<Piece> piecesToMove = new List<Piece>();

        if (piecesAreMoving)
            return;

        int size = pieceBoard.GetLength(0);

        switch (md)
        {
            // starts ignoring the first line
            case InputHandler.MoveDirection.down:
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (pieceBoard[i, j] != null && (pieceBoard[i, k] == null || pieceBoard[i,k].pieceType == Piece.PieceType.Goal))
                            {
                                if (pieceBoard[i, j].pieceType != Piece.PieceType.Static && pieceBoard[i, j].pieceType != Piece.PieceType.Goal)
                                {
                                    pieceBoard[i, k] = pieceBoard[i, j];
                                    pieceBoard[i, j] = null;
                                    pieceBoard[i, k].SetDesiredPosition(new Vector2(i, k));
                                    if (!piecesToMove.Contains(pieceBoard[i, k]))
                                        piecesToMove.Add(pieceBoard[i, k]);
                                    j--;
                                }
                            }
                            else
                                break;
                        }
                    }
                }
                break;

            case InputHandler.MoveDirection.Up:
                for (int i = 0; i < size; i++)
                {
                    for (int j = size - 2; j >= 0; j--)
                    {
                        for (int k = j + 1; k <= size - 1; k++)
                        {
                            if (pieceBoard[i, j] != null && (pieceBoard[i, k] == null || pieceBoard[i, k].pieceType == Piece.PieceType.Goal))
                            {
                                if (pieceBoard[i, j].pieceType != Piece.PieceType.Static && pieceBoard[i, j].pieceType != Piece.PieceType.Goal)
                                {
                                    pieceBoard[i, k] = pieceBoard[i, j];
                                    pieceBoard[i, j] = null;
                                    pieceBoard[i, k].SetDesiredPosition(new Vector2(i, k));
                                    if (!piecesToMove.Contains(pieceBoard[i, k]))
                                        piecesToMove.Add(pieceBoard[i, k]);
                                    j++;
                                }
                            }
                            else
                                break;
                        }
                    }
                }
                break;

            case InputHandler.MoveDirection.left:
                for (int i = 1; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        for (int k = i - 1; k >= 0; k--)
                        {
                            if (pieceBoard[i, j] != null && (pieceBoard[k, j] == null || pieceBoard[k, j].pieceType == Piece.PieceType.Goal))
                            {
                                if (pieceBoard[i, j].pieceType != Piece.PieceType.Static && pieceBoard[i, j].pieceType != Piece.PieceType.Goal)
                                {
                                    pieceBoard[k, j] = pieceBoard[i, j];
                                    pieceBoard[i, j] = null;
                                    pieceBoard[k, j].SetDesiredPosition(new Vector2(k, j));
                                    if (!piecesToMove.Contains(pieceBoard[k, j]))
                                        piecesToMove.Add(pieceBoard[k, j]);
                                    i--;
                                }
                            }
                            else
                                break;
                        }
                    }
                }
                break;

            case InputHandler.MoveDirection.right:
                for (int i = size - 2; i >= 0; i--)
                {
                    for (int j = 0; j < size; j++)
                    {
                        for (int k = i + 1; k <= pieceBoard.GetLength(0) - 1; k++)
                        {
                            if (pieceBoard[i, j] != null && (pieceBoard[k, j] == null || pieceBoard[k, j].pieceType == Piece.PieceType.Goal))
                            {
                                if (pieceBoard[i, j].pieceType != Piece.PieceType.Static && pieceBoard[i, j].pieceType != Piece.PieceType.Goal)
                                {
                                    pieceBoard[k, j] = pieceBoard[i, j];
                                    pieceBoard[i, j] = null;
                                    pieceBoard[k, j].SetDesiredPosition(new Vector2(k, j));
                                    if (!piecesToMove.Contains(pieceBoard[k, j]))
                                        piecesToMove.Add(pieceBoard[k, j]);
                                    i++;
                                }
                            }
                            else
                                break;
                        }
                    }
                }
                break;
        }

        piecesAreMoving = true;

        int piecesToMoveCount = piecesToMove.Count;

        if (piecesToMoveCount == 0)
            EndedPiecesMovement();

        for (int i = 0; i < piecesToMoveCount; i++)
        {
            piecesToMove[i].StartCoroutine("Move");
        }
    }

    public void EndedPiecesMovement()
    {
        piecesAreMoving = false;
        VerifyIfWin();
    }

    public void VerifyIfWin()
    {
        if(mainPiecePosBoard == goalPosBoard)
            Debug.Log("Ganhas te!");
    }
}