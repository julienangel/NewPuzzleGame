using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    const float detectionOffset = -0.5f;

    public static LevelEditorManager editorManager;

    public GameObject piecePrefab;

    GameManager gameManager;
    BoardManager bm;

    Piece[,] pieceBoard;

    Color32 NONEDITINGCOLOR = Color.red;
    Color32 EDITINGCOLOR = Color.green;

    [SerializeField]
    Button LevelEditorBtn, mixBoardBtn, executeSolutionBtn, saveLevelBtn, loadLevelBtn, stopSolution;

    //aux variables
    GameManager.GameState previousState;

    // Use this for initialization
    void Start()
    {
        editorManager = this;
        //deactivate this button when exported to platforms
        if (!Application.isEditor)
        {
            LevelEditorBtn.gameObject.SetActive(false);
            return;
        }

        gameManager = GameManager.gameManager;
        bm = gameManager.GetBoardManager();

        pieceBoard = bm.pieceBoard;

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

        if (gameManager.GetGameState() != GameManager.GameState.Editor)
        {
            cb.pressedColor = cb.highlightedColor = cb.normalColor = EDITINGCOLOR;
            previousState = gameManager.GetGameState();
            gameManager.SetGameState(GameManager.GameState.Editor);
        }

        else
        {
            cb.pressedColor = cb.highlightedColor = cb.normalColor = NONEDITINGCOLOR;
            gameManager.SetGameState(previousState);
        }

        LevelEditorBtn.colors = cb;
    }

    public void SetDefaultStart()
    {
        ColorBlock cb = LevelEditorBtn.colors;
        if (gameManager.GetGameState() != GameManager.GameState.Editor)
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
        gameManager.StartCoroutine(ExecuteSolution());
    }

    public void SaveLevelFunc(Level newLevel)
    {
        LevelJsonManager.SaveInJson(newLevel, "Level0");
    }

    public void LoadLevelFunc()
    {
        Level loadedLevel = LevelJsonManager.LoadFromJson(0);

        bm.CleanEverything();
        bm.CreateBoard(loadedLevel.size);

        int pieceListCount = loadedLevel.PieceList.Count;
        for (int i = 0; i < pieceListCount; i++)
        {
            PieceInfo newPiece = loadedLevel.PieceList[i];
            bm.InstantiateGameObject(newPiece.pieceType, newPiece.pos);
        }
    }

    public void StopSolution()
    {
        gameManager.StopCoroutine(ExecuteSolution());
        LoadLevelFunc();
    }

    public void EditOrPlacePiece(Vector2 mousePosClick)
    {
        if (mousePosClick.x < Vector2.zero.x + detectionOffset || mousePosClick.y < Vector2.zero.y + detectionOffset ||
            mousePosClick.x > bm.pieceBoard.GetLength(0) + detectionOffset || mousePosClick.y > bm.pieceBoard.GetLength(1) + detectionOffset)
            return;

        Vector2 vecRoundToInt = new Vector2(Mathf.RoundToInt(mousePosClick.x), Mathf.RoundToInt(mousePosClick.y));

        if (bm.pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y] != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GameObject.Destroy(bm.pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y].gameObject);
                pieceBoard[(int)vecRoundToInt.x, (int)vecRoundToInt.y] = null;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                bm.InstantiateGameObject(PieceType.normal, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                bm.InstantiateGameObject(PieceType.statice, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                bm.InstantiateGameObject(PieceType.goal, vecRoundToInt);
            }
            else if (Input.GetKey(KeyCode.F))
            {
                bm.InstantiateGameObject(PieceType.objective, vecRoundToInt);
            }
        }
    }

    public void CreateRandomLevel()
    {
        bm.CleanEverything();
        bm.CreateBoard(6);

        pieceBoard = bm.pieceBoard;

        //creating random movable objects
        for (int i = 0; i < (int)pieceBoard.GetLength(0) / 2; i++)
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = bm.InstantiateGameObject(PieceType.normal, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //creating random static objects
        for (int j = 0; j < (int)pieceBoard.GetLength(0) / 2; j++)
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = bm.InstantiateGameObject(PieceType.statice, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //creating goal piece
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = bm.InstantiateGameObject(PieceType.goal, new Vector2((int)randomPos.x, (int)randomPos.y));
        }

        //Create the main piece
        {
            Vector2 randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            while (pieceBoard[(int)randomPos.x, (int)randomPos.y] != null)
                randomPos = new Vector2((int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)), (int)UnityEngine.Random.Range(0, pieceBoard.GetLength(0)));

            pieceBoard[(int)randomPos.x, (int)randomPos.y] = bm.InstantiateGameObject(PieceType.objective, new Vector2((int)randomPos.x, (int)randomPos.y));
        }
    }

    public IEnumerator ExecuteSolution()
    {
        int sizeToSearch = 20;
        pieceBoard = bm.pieceBoard;
        gameManager.SetGameState(GameManager.GameState.Solving);
        Level newLevel = new Level(pieceBoard.GetLength(0));
        bool foundSolution = false;
        List<InputHandler.MoveDirection> solutionBoardTemp = new List<InputHandler.MoveDirection>(sizeToSearch);
        sizeToSearch = 20;

        List<InputHandler.MoveDirection> directionsAble = new List<InputHandler.MoveDirection>
            {
                InputHandler.MoveDirection.down,
                InputHandler.MoveDirection.Up,
                InputHandler.MoveDirection.right,
                InputHandler.MoveDirection.left
            };

        for (int r = 0; r < 1000; r++)
        {
            solutionBoardTemp = new List<InputHandler.MoveDirection>(sizeToSearch);
            int mixedBoardLength = solutionBoardTemp.Capacity;

            List<InputHandler.MoveDirection> verticalDirection = new List<InputHandler.MoveDirection>
            {
                InputHandler.MoveDirection.down,
                InputHandler.MoveDirection.Up
            };

            List<InputHandler.MoveDirection> horizontalDirection = new List<InputHandler.MoveDirection>
            {
                InputHandler.MoveDirection.right,
                InputHandler.MoveDirection.left
            };

            for (int i = 0; i < mixedBoardLength && !bm.VerifyIfWin(); i++)
            {
                InputHandler.MoveDirection dir = directionsAble[UnityEngine.Random.Range(0, directionsAble.Count)];

                if (verticalDirection.Contains(dir))
                {
                    directionsAble = new List<InputHandler.MoveDirection>();
                    directionsAble.Add(InputHandler.MoveDirection.right);
                    directionsAble.Add(InputHandler.MoveDirection.left);
                }
                else
                {
                    directionsAble = new List<InputHandler.MoveDirection>();
                    directionsAble.Add(InputHandler.MoveDirection.Up);
                    directionsAble.Add(InputHandler.MoveDirection.down);
                }

                yield return gameManager.StartCoroutine(bm.MovePieces(dir));

                if(!bm.DidMove())
                {
                    if (verticalDirection.Contains(dir))
                    {
                        if (dir == InputHandler.MoveDirection.Up)
                            dir = InputHandler.MoveDirection.down;
                        else
                            dir = InputHandler.MoveDirection.Up;
                    }
                    else
                    {
                        if (dir == InputHandler.MoveDirection.right)
                            dir = InputHandler.MoveDirection.left;
                        else
                            dir = InputHandler.MoveDirection.right;
                    }
                    yield return gameManager.StartCoroutine(bm.MovePieces(dir));
                }

                //directionsAble.Remove(dir);

                solutionBoardTemp.Add(dir);

                if (i != 0)
                {
                    directionsAble.Add(solutionBoardTemp[solutionBoardTemp.Count - 2]);
                }

                //yield return new WaitForSeconds(Piece.PIECE_TIME_VELOCITY);
                yield return new WaitForEndOfFrame();
            }

            if (solutionBoardTemp.Count < sizeToSearch && bm.VerifyIfWin())
            {
                Debug.Log("Encontrou uma solução mais pequena : " + solutionBoardTemp.Count);

                newLevel.directionListSolution.Clear();
                newLevel.directionListSolution = new List<InputHandler.MoveDirection>(solutionBoardTemp);

                foundSolution = true;

                sizeToSearch = solutionBoardTemp.Count;
            }

            solutionBoardTemp.Clear();

            yield return new WaitForEndOfFrame();

            LoadLevelFunc();

            yield return new WaitForEndOfFrame();

            GC.Collect();
        }

        if (foundSolution)
        {
            SaveLevel(newLevel);
        }

        yield return new WaitForSeconds(.2f);
        sizeToSearch = 20;
        gameManager.SetGameState(GameManager.GameState.InGame);
    }

    public void SaveLevel(Level newLevel)
    {
        pieceBoard = bm.pieceBoard;
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

        SaveLevelFunc(newLevel);
    }

    public void SaveLevel()
    {
        pieceBoard = bm.pieceBoard;
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

        SaveLevelFunc(level);
    }
}