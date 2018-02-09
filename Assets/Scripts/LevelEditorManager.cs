using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    public static LevelEditorManager editorManager;

    public GameObject piecePrefab;

    GameManager gm;

    Color32 NONEDITINGCOLOR = Color.red;
    Color32 EDITINGCOLOR = Color.green;

    [SerializeField]
    Button LevelEditorBtn, mixBoardBtn, executeSolutionBtn, saveLevelBtn, loadLevelBtn;

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

        SetDefaultStart();

        LevelEditorBtn.onClick.AddListener(delegate () { ChangeEditorState(); });
        mixBoardBtn.onClick.AddListener(delegate () { gm.boardManager.CreateRandomLevel(); });
        executeSolutionBtn.onClick.AddListener(delegate () { ExecuteSolutionFunc(); });
        saveLevelBtn.onClick.AddListener(delegate () { gm.boardManager.SaveLevel(); });
        loadLevelBtn.onClick.AddListener(delegate () { LoadLevelFunc(); });
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
        gm.StartCoroutine(gm.boardManager.ExecuteSolution());
    }

    public void SaveLevelFunc(Level newLevel)
    {
        LevelJsonManager.SaveInJson(newLevel, "Level0");
    }

    public void LoadLevelFunc()
    {
        Level loadedLevel = LevelJsonManager.LoadFromJson(0);

        gm.boardManager.CleanEverything();
        gm.boardManager.CreateBoard(loadedLevel.size);

        int pieceListCount = loadedLevel.PieceList.Count;
        for (int i = 0; i < pieceListCount; i++)
        {
            PieceInfo newPiece = loadedLevel.PieceList[i];
            switch (newPiece.pieceType)
            {
                case Piece.PieceType.Goal:
                    gm.boardManager.InstantiateGameObject(newPiece.pieceType, newPiece.pos);
                    break;

                case Piece.PieceType.MainPiece:
                    gm.boardManager.InstantiateGameObject(newPiece.pieceType, newPiece.pos);
                    break;

                case Piece.PieceType.Playable:
                    gm.boardManager.InstantiateGameObject(newPiece.pieceType, newPiece.pos);
                    break;

                case Piece.PieceType.Static:
                    gm.boardManager.InstantiateGameObject(newPiece.pieceType, newPiece.pos);
                    break;
            }
        }
    }
}