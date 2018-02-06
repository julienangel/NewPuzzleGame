using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    public GameObject piecePrefab;

    GameManager gm;

    Color32 NONEDITINGCOLOR = Color.red;
    Color32 EDITINGCOLOR = Color.green;

    [SerializeField]
    Button LevelEditorBtn, mixBoardBtn, executeSolutionBtn;

    //aux variables
    GameManager.GameState previousState;

    // Use this for initialization
    void Start()
    {
        //deactivate this button when exported to platforms
        if (!Application.isEditor)
        {
            LevelEditorBtn.gameObject.SetActive(false);
            return;
        }

        gm = GameManager.gameManager;
        SetDefaultStart();
        //gm.eventManager.AddToList(ChangeEditorState);
        LevelEditorBtn.onClick.AddListener(delegate () { ChangeEditorState(); });
        mixBoardBtn.onClick.AddListener(delegate () { MixBoardFunc(); });
        executeSolutionBtn.onClick.AddListener(delegate () { ExecuteSolutionFunc(); });
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

    public void MixBoardFunc()
    {
        gm.StartCoroutine(gm.boardManager.MixBoard());
    }

    public void ExecuteSolutionFunc()
    {
        gm.StartCoroutine(gm.boardManager.ExecuteSolution());
    }
}