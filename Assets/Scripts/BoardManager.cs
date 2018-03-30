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

    public BoardManager()
    {
        //this.mono = mono;
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
                InstantiateGameObject(PieceType.normal, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                InstantiateGameObject(PieceType.statice, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                InstantiateGameObject(PieceType.goal, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.F))
            {
                InstantiateGameObject(PieceType.objective, vecRoundToInt);
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
    }

    public bool VerifyIfWin()
    {
        if (objectivePosBoard == goalPosBoard)
            return true;
        return false;
    }

    public void CreateRandomLevel()
    {
        CleanEverything();
        CreateBoard(6);

        //creating random movable objects
        for (int i = 0; i < (int)pieceBoard.GetLength(0) / 2; i++)
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = InstantiateGameObject(PieceType.normal, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //creating random static objects
        for (int j = 0; j < (int)pieceBoard.GetLength(0) / 2; j++)
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = InstantiateGameObject(PieceType.statice, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //creating goal piece
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = InstantiateGameObject(PieceType.goal, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //Create the main piece
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = InstantiateGameObject(PieceType.objective, new Vector2((int)randomPos.x, (int)randomPos.y));
        }
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

    int levelNumber = 0;
    bool foundSolution = false;
    public IEnumerator ExecuteSolution()
    {
        GameManager.gameManager.gameState = GameManager.GameState.Solving;
        Level newLevel = new Level(pieceBoard.GetLength(0));
        foundSolution = false;
        List<InputHandler.MoveDirection> solutionBoardTemp = new List<InputHandler.MoveDirection>(sizeToSearch);
        sizeToSearch = 20;

        for (int r = 0; r < 1000; r++)
        {
            solutionBoardTemp = new List<InputHandler.MoveDirection>(sizeToSearch);
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

            if (solutionBoardTemp.Count < sizeToSearch && VerifyIfWin())
            {
                Debug.Log("Encontrou uma solução mais pequena : " + solutionBoardTemp.Count);

                newLevel.directionListSolution.Clear();
                newLevel.directionListSolution = new List<InputHandler.MoveDirection>(solutionBoardTemp);

                foundSolution = true;

                sizeToSearch = solutionBoardTemp.Count;
            }

            solutionBoardTemp.Clear();

            yield return new WaitForEndOfFrame();

            LevelEditorManager.editorManager.LoadLevelFunc();

            yield return new WaitForEndOfFrame();

            GC.Collect();
        }

        if (foundSolution)
        {
            SaveLevel(newLevel);
        }

        yield return new WaitForSeconds(.2f);
        sizeToSearch = 20;
        GameManager.gameManager.gameState = GameManager.GameState.InGame;
    }

    void SaveLevel(Level newLevel)
    {
        int size = newLevel.size;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (pieceBoard[i, j] != null)
                {
                    PieceInfo pieceInfo = new PieceInfo(new Vector2(i, j), pieceBoard[i, j].GetPieceType());
                    newLevel.AddPieceElement(pieceInfo);
                }
            }
        }

        LevelEditorManager.editorManager.SaveLevelFunc(newLevel);
    }

    public void SaveLevel()
    {
        int size = pieceBoard.GetLength(0);
        Level level = new Level(size);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (pieceBoard[i, j] != null)
                {
                    PieceInfo pieceInfo = new PieceInfo(new Vector2(i, j), pieceBoard[i, j].GetPieceType());
                    level.AddPieceElement(pieceInfo);
                }
            }
        }

        LevelEditorManager.editorManager.SaveLevelFunc(level);
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