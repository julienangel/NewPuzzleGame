using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager
{
    const int sizeToSearch = 20;
    public List<InputHandler.MoveDirection> solutionBoardList = new List<InputHandler.MoveDirection>(sizeToSearch);

    const float detectionOffset = -0.5f;

    public Piece[,] pieceBoard;
    public GameObject[,] backgroundBoard;
    public List<PieceInfo> pieceList = new List<PieceInfo>();
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
        //PlaceBackgroundPieces(size);
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


        for (int i = 0; i < 20; i++)
        {
            solutionBoardList.Add(InputHandler.MoveDirection.left);
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
                InstantiateGameObject(Piece.PieceType.Playable, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                InstantiateGameObject(Piece.PieceType.Static, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                InstantiateGameObject(Piece.PieceType.MainPiece, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.F))
            {
                InstantiateGameObject(Piece.PieceType.Goal, vecRoundToInt);
            }
        }
    }

    public Piece InstantiateGameObject(Piece.PieceType pieceType, Vector2 pos)
    {
        PieceInfo newPiece;
        //return pieceBoard[(int)pos.x, (int)pos.y] = GameObject.Instantiate(objectToInstantiate, new Vector2((int)pos.x, (int)pos.y), Quaternion.identity).GetComponent<Piece>();
        switch (pieceType)
        {
            case Piece.PieceType.Goal:
                pieceList.Add(newPiece = new PieceInfo(pos, pieceType));
                return pieceBoard[(int)pos.x, (int)pos.y] = GameObject.Instantiate(goalPiece, pos, Quaternion.identity).GetComponent<Piece>();

            case Piece.PieceType.MainPiece:
                pieceList.Add(newPiece = new PieceInfo(pos, pieceType));
                return pieceBoard[(int)pos.x, (int)pos.y] = GameObject.Instantiate(mainPiece, pos, Quaternion.identity).GetComponent<Piece>();

            case Piece.PieceType.Playable:
                pieceList.Add(newPiece = new PieceInfo(pos, pieceType));
                return pieceBoard[(int)pos.x, (int)pos.y] = GameObject.Instantiate(movablePiece, pos, Quaternion.identity).GetComponent<Piece>();

            case Piece.PieceType.Static:
                pieceList.Add(newPiece = new PieceInfo(pos, pieceType));
                return pieceBoard[(int)pos.x, (int)pos.y] = GameObject.Instantiate(staticPiece, pos, Quaternion.identity).GetComponent<Piece>();
        }
        return null;
    }

    public void ReloadLevel()
    {
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

        CleanEverything();

        int pieceListCount = pieceList.Count;
        for (int i = 0; i < pieceListCount; i++)
        {
            Vector2 pos = pieceList[i].pos;
            switch (pieceList[i].pieceType)
            {
                case Piece.PieceType.Goal:
                    pieceBoard[(int)pos.x, (int)pos.y] = GameObject.Instantiate(goalPiece, pos, Quaternion.identity).GetComponent<Piece>();
                    break;

                case Piece.PieceType.MainPiece:
                    pieceBoard[(int)pos.x, (int)pos.y] = GameObject.Instantiate(mainPiece, pos, Quaternion.identity).GetComponent<Piece>();
                    break;

                case Piece.PieceType.Playable:
                    pieceBoard[(int)pos.x, (int)pos.y] = GameObject.Instantiate(movablePiece, pos, Quaternion.identity).GetComponent<Piece>();
                    break;

                case Piece.PieceType.Static:
                    pieceBoard[(int)pos.x, (int)pos.y] = GameObject.Instantiate(staticPiece, pos, Quaternion.identity).GetComponent<Piece>();
                    break;
            }
        }
    }

    public IEnumerator MovePieces(InputHandler.MoveDirection md)
    {
        List<Piece> piecesToMove = new List<Piece>();

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
                        for (int k = j - 1; k >= 0; k--)
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

    public bool VerifyIfWin()
    {
        if (mainPiecePosBoard == goalPosBoard)
            return true;
        return false;
    }

    public void CreateRandomLevel()
    {
        CleanEverything();
        CreateBoard(5);

        //creating random movable objects
        for (int i = 0; i < (int)pieceBoard.GetLength(0) / 2; i++)
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = InstantiateGameObject(Piece.PieceType.Playable, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //creating random static objects
        for (int j = 0; j < (int)pieceBoard.GetLength(0) / 2; j++)
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = InstantiateGameObject(Piece.PieceType.Static, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //creating goal piece
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = InstantiateGameObject(Piece.PieceType.Goal, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //Create the main piece
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = InstantiateGameObject(Piece.PieceType.MainPiece, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //GameManager.gameManager.StartCoroutine(ExecuteSolution());
    }

    public void CleanEverything()
    {
        //Cleaning background pieces
        //for (int i = 0; i < backgroundBoard.GetLength(0); i++)
        //{
        //    for (int j = 0; j < backgroundBoard.GetLength(1); j++)
        //    {
        //        GameObject.Destroy(backgroundBoard[i, j]);
        //        backgroundBoard[i, j] = null;
        //    }
        //}

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
    }

    int levelNumber = 0;
    bool foundSolution = false;
    public IEnumerator ExecuteSolution()
    {
        foundSolution = false;
        for (int r = 0; r < 100; r++)
        {
            List<InputHandler.MoveDirection> solutionBoardTemp = new List<InputHandler.MoveDirection>(sizeToSearch);
            int mixedBoardLength = solutionBoardTemp.Capacity;

            List<InputHandler.MoveDirection> directionsAble = new List<InputHandler.MoveDirection>
            {
                InputHandler.MoveDirection.down,
                InputHandler.MoveDirection.Up,
                InputHandler.MoveDirection.right,
                InputHandler.MoveDirection.left
            };

            for (int i = 0; i < mixedBoardLength && !VerifyIfWin(); i++)
            {
                InputHandler.MoveDirection dir = directionsAble[UnityEngine.Random.Range(0, directionsAble.Count)];

                GameManager.gameManager.StartCoroutine(MovePieces(dir));

                directionsAble.Remove(dir);

                solutionBoardTemp.Add(dir);

                if (i != 0)
                {
                    directionsAble.Add(solutionBoardTemp[solutionBoardTemp.Count - 2]);
                }

                //yield return new WaitForSeconds(Piece.PIECE_TIME_VELOCITY);
                yield return new WaitForEndOfFrame();
            }

            if (solutionBoardTemp.Count < solutionBoardList.Count && VerifyIfWin())
            {
                Debug.Log("Encontrou uma solução mais pequena : " + solutionBoardTemp.Count);
                solutionBoardList = new List<InputHandler.MoveDirection>(solutionBoardTemp);
                for (int h = 0; h < solutionBoardList.Count; h++)
                {
                    foundSolution = true;
                    Debug.Log(solutionBoardList[h]);
                }
            }
            solutionBoardTemp.Clear();

            //ReloadLevel();
            for (int y = 0; y < pieceBoard.GetLength(0); y++)
            {
                for (int x = 0; x < pieceBoard.GetLength(1); x++)
                {
                    if (pieceBoard[x, y] != null)
                    {
                        pieceBoard[x, y].BackToOriginalPosition();
                    }
                }
            }

            yield return new WaitForSeconds(.1f);

            GC.Collect();
        }
        if (foundSolution)
            LevelEditorManager.editorManager.SaveLevelFunc();

        yield return new WaitForSeconds(2f);
    }
}