using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LevelEditorManager : MonoBehaviour
{
    const float detectionOffset = -0.5f;

    private static LevelEditorManager instance;
    public static LevelEditorManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelEditorManager>();
            }
            return instance;
        }
    }

    EditorManager editorManager;
    BoardManager boardManager;

    Level currentlevel;

    Piece[,] pieceBoard;

    Color32 NONEDITINGCOLOR = Color.red;
    Color32 EDITINGCOLOR = Color.green;

    [SerializeField]
    Button LevelEditorBtn, mixBoardBtn, executeSolutionBtn, saveLevelBtn, loadLevelBtn, stopSolution;

    //aux variables
    EditorManager.GameState previousState;

    int levelCount = 0;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        //deactivate this button when exported to platforms
        if (!Application.isEditor)
        {
            LevelEditorBtn.gameObject.SetActive(false);
        }

        levelCount = Resources.LoadAll("Levels").Length-1;

        editorManager = EditorManager.Instance;
        boardManager = editorManager.GetBoardManager();

        pieceBoard = boardManager.pieceBoard;

        SetDefaultStart();

        LevelEditorBtn.onClick.AddListener(delegate () { ChangeEditorState(); });
        mixBoardBtn.onClick.AddListener(delegate () { CreateRandomLevel(); });
        executeSolutionBtn.onClick.AddListener(delegate () { ExecuteSolutionFunc(); });
        saveLevelBtn.onClick.AddListener(delegate () { SaveLevel(); });
        loadLevelBtn.onClick.AddListener(delegate () { LoadLevelFunc(); });
        stopSolution.onClick.AddListener(delegate () { StopSolution(); });
    }

    public void ChangeEditorState()
    {
        ColorBlock cb = LevelEditorBtn.colors;

        if (editorManager.GetGameState() != EditorManager.GameState.Editor)
        {
            cb.pressedColor = cb.highlightedColor = cb.normalColor = EDITINGCOLOR;
            previousState = editorManager.GetGameState();
            editorManager.SetGameState(EditorManager.GameState.Editor);
        }

        else
        {
            cb.pressedColor = cb.highlightedColor = cb.normalColor = NONEDITINGCOLOR;
            editorManager.SetGameState(previousState);
        }

        LevelEditorBtn.colors = cb;
    }

    public void SetDefaultStart()
    {
        ColorBlock cb = LevelEditorBtn.colors;
        if (editorManager.GetGameState() != EditorManager.GameState.Editor)
        {
            cb.pressedColor = cb.highlightedColor = cb.normalColor = NONEDITINGCOLOR;
        }
        else
        {
            cb.pressedColor = cb.highlightedColor = cb.normalColor = EDITINGCOLOR;
        }

        LevelEditorBtn.colors = cb;
    }

    public void ExecuteSolutionFunc()
    {
        ExecuteSolution();
    }

    public void SaveLevelFuncTemp(Level newLevel)
    {
        LevelJsonManager.SaveInJson(newLevel, "Temp");
    }

    public void SaveLevelFunc(Level newLevel)
    {
        LevelJsonManager.SaveInJson(newLevel, "Level" + levelCount++);
    }

    public void LoadLevelFunc()
    {
        Level loadedLevel = LevelJsonManager.LoadFromJson(0);

        boardManager.CleanEverything();
        boardManager.CreateBoard(loadedLevel.size);

        int pieceListCount = loadedLevel.PieceList.Count;
        for (int i = 0; i < pieceListCount; i++)
        {
            PieceInfo newPiece = loadedLevel.PieceList[i];
            boardManager.InstantiateGameObject(newPiece.pieceType, newPiece.pos);
        }
    }

    public void StopSolution()
    {
        //ExecuteSolution();
        //LoadLevelFunc();
    }

    public void EditOrPlacePiece(Vector2 mousePosClick)
    {
        if (mousePosClick.x < Vector2.zero.x + detectionOffset || mousePosClick.y < Vector2.zero.y + detectionOffset ||
            mousePosClick.x > boardManager.pieceBoard.GetLength(0) + detectionOffset || mousePosClick.y > boardManager.pieceBoard.GetLength(1) + detectionOffset)
            return;

        Vector2 vecRoundToInt = new Vector2(Mathf.RoundToInt(mousePosClick.x), Mathf.RoundToInt(mousePosClick.y));

        if (boardManager.pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y] != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GameObject.Destroy(boardManager.pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y].gameObject);
                pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y] = null;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                boardManager.InstantiateGameObject(PieceType.normal, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                boardManager.InstantiateGameObject(PieceType.statice, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                boardManager.InstantiateGameObject(PieceType.goal, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.F))
            {
                boardManager.InstantiateGameObject(PieceType.objective, vecRoundToInt);
            }
        }
    }

    public void CreateRandomLevel()
    {
        boardManager.CleanEverything();
        boardManager.CreateBoard(6);

        pieceBoard = boardManager.pieceBoard;

        //creating random movable objects
        for (int i = 0; i < (int)pieceBoard.GetLength(0) / 2; i++)
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = boardManager.InstantiateGameObject(PieceType.normal, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //creating random static objects
        for (int j = 0; j < (int)pieceBoard.GetLength(0) / 2; j++)
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = boardManager.InstantiateGameObject(PieceType.statice, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //creating goal piece
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = boardManager.InstantiateGameObject(PieceType.goal, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //Create the main piece
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = boardManager.InstantiateGameObject(PieceType.objective, new Vector2((int)randomPos.x, (int)randomPos.y));
            boardManager.SetGoalPiecePos(new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        Invoke("ExecuteSolution", 0.1f);
    }

    public void ExecuteSolution()
    {
        editorManager.SetGameState(EditorManager.GameState.Solving);

        pieceBoard = boardManager.pieceBoard;

        PieceType[] board = new PieceType[pieceBoard.Length];
        int k = 0;
        for (int i = pieceBoard.GetLength(1) - 1; i >= 0; i--)
        {

            for (int j = 0; j < pieceBoard.GetLength(0); j++)
            {
                Piece x = pieceBoard[j, i];
                board[k++] = x && x.GetPieceType() != PieceType.objective ? x.GetPieceType() : PieceType.empty;

            }
        }

        LevelSolver newLevelSolver = new LevelSolver(pieceBoard.GetLength(0), pieceBoard.GetLength(1), board);
        newLevelSolver.SetGoals(new Tuple<int, int>(pieceBoard.GetLength(0) - (int)boardManager.GetObjectivePos().y - 1, (int)boardManager.GetObjectivePos().x));

        List<InputHandler.MoveDirection> moveDirections = newLevelSolver.Solve();
        if (moveDirections.Count > 0)
        {
            print(String.Join(" ", moveDirections.Select(x => x.ToString())));
            SaveLevelTemp(moveDirections);
        }
        else
        {
            print("Nao tem solução!");
        }
        editorManager.SetGameState(EditorManager.GameState.InGame);
    }

    public void SaveLevel()
    {
        //pieceBoard = boardManager.pieceBoard;
        //int size = pieceBoard.GetLength(0);
        //Level level = new Level(size);

        //for (int i = 0; i < size; i++)
        //{
        //    for (int j = 0; j < size; j++)
        //    {
        //        if (pieceBoard[i, j] != null)
        //        {
        //            PieceInfo pieceInfo = new PieceInfo(new Vector2(i, j), pieceBoard[i, j].GetPieceType());
        //            level.AddPieceElement(pieceInfo);
        //        }
        //    }
        //}

        //currentlevel = level;
        SaveLevelFunc(currentlevel);
    }

    public void SaveLevelTemp(List<InputHandler.MoveDirection> directions)
    {
        pieceBoard = boardManager.pieceBoard;
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
        level.directionListSolution = directions;

        currentlevel = level;
        SaveLevelFuncTemp(level);
    }

    public void SaveLevelTemp()
    {
        pieceBoard = boardManager.pieceBoard;
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

        currentlevel = level;
        SaveLevelFuncTemp(level);
    }
}