using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager
{
    int sizeToSearch = 20;

    const float detectionOffset = -0.5f;

    public Piece[,] pieceBoard;
    public GameObject[,] backgroundBoard;
    public List<PieceInfo> pieceList = new List<PieceInfo>();

    private GoalPiece goalPiece;
    private ObjectivePiece objectivePiece;

    private Vector2 goalPosBoard;
    private Vector2 objectivePosBoard;

    public NormalPiece movablePiecePrefab = null;
    public GoalPiece goalPiecePrefab = null;
    public StaticPiece staticPiecePrefab = null;
    public GameObject backgroundPiece = null;
    public ObjectivePiece objectivePiecePrefab = null;

    private bool piecesAreMoving = false;

    testingManager gameManager;

    public BoardManager(testingManager gameManager)
    {
        //this.mono = mono;
        this.gameManager = gameManager;
        LoadPrefabs();
    }

    void LoadPrefabs()
    {
        movablePiecePrefab = Resources.Load<NormalPiece>("Prefabs/GamePieces/NormalPiece");
        backgroundPiece = Resources.Load<GameObject>("Prefabs/GamePieces/Background");
        staticPiecePrefab = Resources.Load<StaticPiece>("Prefabs/GamePieces/StaticPiece");
        goalPiecePrefab = Resources.Load<GoalPiece>("Prefabs/GamePieces/Goal");
        objectivePiecePrefab = Resources.Load<ObjectivePiece>("Prefabs/GamePieces/Objective");
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

    public Piece InstantiateGameObject(PieceType classe, Vector2 pos)
    {
        PieceInfo newPiece;
        Component component;
        pieceList.Add(newPiece = new PieceInfo(pos, classe));
        switch (classe)
        {
            case PieceType.normal:
                component = movablePiecePrefab;
                break;
            case PieceType.statice:
                component = staticPiecePrefab;
                break;
            case PieceType.goal:
                component = goalPiecePrefab;
                break;
            case PieceType.objective:
                component = objectivePiecePrefab;
                break;
            default:
                component = null;
                break;
        }
        return pieceBoard[(int)pos.x, (int)pos.y] = (Piece)GameObject.Instantiate(component, pos, Quaternion.identity);
    }

    public IEnumerator MovePieces(InputHandler.MoveDirection md)
    {
        List<MovablePiece> piecesToMove = new List<MovablePiece>();

        if (piecesAreMoving)
            yield break;

        int size = pieceBoard.GetLength(0);

        switch (md)
        {
            // starts ignoring the first line
            case InputHandler.MoveDirection.down:
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (pieceBoard[i, j] != null)
                        {
                            for (int k = j - 1; k >= 0; k--)
                            {
                                if (pieceBoard[i, k] == null || pieceBoard[i, k].GetComponent<ObjectivePiece>())
                                {
                                    if (pieceBoard[i, j].GetComponent<MovablePiece>())
                                    {
                                        pieceBoard[i, k] = pieceBoard[i, j];
                                        pieceBoard[i, j] = null;
                                        pieceBoard[i, k].GetComponent<MovablePiece>().SetDesiredPosition(new Vector2(i, k));
                                        if (!piecesToMove.Contains(pieceBoard[i, k] as MovablePiece))
                                            piecesToMove.Add(pieceBoard[i, k] as MovablePiece);
                                        j--;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                    }
                }
                break;

            case InputHandler.MoveDirection.Up:
                for (int i = 0; i < size; i++)
                {
                    for (int j = size - 2; j >= 0; j--)
                    {
                        if (pieceBoard[i, j] != null)
                        {
                            for (int k = j + 1; k <= size - 1; k++)
                            {
                                if (pieceBoard[i, k] == null || pieceBoard[i, k].GetComponent<ObjectivePiece>())
                                {
                                    if (pieceBoard[i, j].GetComponent<MovablePiece>())
                                    {
                                        pieceBoard[i, k] = pieceBoard[i, j];
                                        pieceBoard[i, j] = null;
                                        pieceBoard[i, k].GetComponent<MovablePiece>().SetDesiredPosition(new Vector2(i, k));
                                        if (!piecesToMove.Contains(pieceBoard[i, k] as MovablePiece))
                                            piecesToMove.Add(pieceBoard[i, k] as MovablePiece);
                                        j++;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                    }
                }
                break;

            case InputHandler.MoveDirection.left:
                for (int i = 1; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (pieceBoard[i, j] != null)
                        {
                            for (int k = i - 1; k >= 0; k--)
                            {
                                if (pieceBoard[k, j] == null || pieceBoard[k, j].GetComponent<ObjectivePiece>())
                                {
                                    if (pieceBoard[i, j].GetComponent<MovablePiece>())
                                    {
                                        pieceBoard[k, j] = pieceBoard[i, j];
                                        pieceBoard[i, j] = null;
                                        pieceBoard[k, j].GetComponent<MovablePiece>().SetDesiredPosition(new Vector2(k, j));
                                        if (!piecesToMove.Contains(pieceBoard[k, j] as MovablePiece))
                                            piecesToMove.Add(pieceBoard[k, j] as MovablePiece);
                                        i--;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                    }
                }
                break;

            case InputHandler.MoveDirection.right:
                for (int i = size - 2; i >= 0; i--)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (pieceBoard[i, j] != null)
                        {
                            for (int k = i + 1; k <= pieceBoard.GetLength(0) - 1; k++)
                            {
                                if (pieceBoard[k, j] == null || pieceBoard[k, j].GetComponent<ObjectivePiece>())
                                {
                                    if (pieceBoard[i, j].GetComponent<MovablePiece>())
                                    {
                                        pieceBoard[k, j] = pieceBoard[i, j];
                                        pieceBoard[i, j] = null;
                                        pieceBoard[k, j].GetComponent<MovablePiece>().SetDesiredPosition(new Vector2(k, j));
                                        if (!piecesToMove.Contains(pieceBoard[k, j] as MovablePiece))
                                            piecesToMove.Add(pieceBoard[k, j] as MovablePiece);
                                        i++;
                                    }
                                }
                                else
                                    break;
                            }
                        }
                    }
                }
                break;
        }

        piecesAreMoving = true;

        piecesToMoveCount = piecesToMove.Count;

        if (piecesToMoveCount == 0)
            EndedPiecesMovement();

        for (int i = 0; i < piecesToMoveCount; i++)
        {
            piecesToMove[i].StartCoroutine("Move");
        }
    }

    int piecesToMoveCount = 0;
    public bool DidMove()
    {
        return piecesToMoveCount > 1;
    }

    public void EndedPiecesMovement()
    {
        piecesAreMoving = false;
    }

    public bool VerifyIfWin()
    {
        if (objectivePosBoard == goalPosBoard)
            return true;
        return false;
    }

    public void CleanEverything()
    {
        //Cleaning background pieces
        if (backgroundBoard.GetLength(0) > 0)
        {
            for (int i = 0; i < backgroundBoard.GetLength(0); i++)
            {
                for (int j = 0; j < backgroundBoard.GetLength(1); j++)
                {
                    GameObject.Destroy(backgroundBoard[i, j]);
                    backgroundBoard[i, j] = null;
                }
            }
        }

        //cleaning all pieces
        for (int y = 0; y < pieceBoard.GetLength(0); y++)
        {
            for (int x = 0; x < pieceBoard.GetLength(1); x++)
            {
                if (pieceBoard[x, y] != null)
                {
                    GameObject.Destroy(pieceBoard[x, y].gameObject);
                    pieceBoard[x, y] = null;
                }
            }
        }

        //just to prevent
        GameObject[] o = GameObject.FindGameObjectsWithTag("Piece");
        for (int i = 0; i < o.Length; i++)
        {
            GameObject.Destroy(o[i]);
        }
    }

    public void SetGoalPiecePos(Vector2 newPos)
    {
        goalPosBoard = newPos;
    }

    public void SetGoalPiece(GoalPiece goalPiece)
    {
        this.goalPiece = goalPiece;
        this.goalPosBoard = this.goalPiece.Position;
    }

    public void SetObjectivePiece(ObjectivePiece objectivePiece)
    {
        this.objectivePiece = objectivePiece;
        this.objectivePosBoard = this.objectivePiece.Position;
    }

    public void SetElementOnBoard(int x, int y, Piece newPiece)
    {
        pieceBoard[x, y] = newPiece;
    }
}