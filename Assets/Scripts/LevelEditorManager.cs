using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    public static LevelEditorManager editorManager;

    public GameObject piecePrefab;

    GameManager gm;
    BoardManager bm;

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

        gm = GameManager.gameManager;
        bm = gm.GetBoardManager();

        SetDefaultStart();

        LevelEditorBtn.onClick.AddListener(delegate () { ChangeEditorState(); });
        mixBoardBtn.onClick.AddListener(delegate () { bm.CreateRandomLevel(); });
        executeSolutionBtn.onClick.AddListener(delegate () { ExecuteSolutionFunc(); });
        saveLevelBtn.onClick.AddListener(delegate () { bm.SaveLevel(); });
        loadLevelBtn.onClick.AddListener(delegate () { LoadLevelFunc(); });
        stopSolution.onClick.AddListener(delegate () { StopSolution(); });
    }

    public void ChangeEditorState()
    {
        ColorBlock cb = LevelEditorBtn.colors;

        if (gm.gameState != GameManager.GameState.Editor)
        {
            cb.pressedColor = cb.highlightedColor = cb.normalColor = EDITINGCOLOR;
            previousState = gm.gameState;
            gm.gameState = GameManager.GameState.Editor;
        }

        else
        {
            cb.pressedColor = cb.highlightedColor = cb.normalColor = NONEDITINGCOLOR;
            gm.gameState = previousState;
        }

        LevelEditorBtn.colors = cb;
    }

    public void SetDefaultStart()
    {
        ColorBlock cb = LevelEditorBtn.colors;
        if (gm.gameState != GameManager.GameState.Editor)
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
        gm.StartCoroutine(bm.ExecuteSolution());
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
        gm.StopAllCoroutines();
        LoadLevelFunc();
    }
}