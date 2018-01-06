using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    GameManager gm;

    Color32 NONEDITINGCOLOR = Color.red;
    Color32 EDITINGCOLOR = Color.green;

    [SerializeField]
    Button LevelEditorBtn;

    enum EditorState
    {
        Editing,
        NonEditing
    };

    EditorState editorState;

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
        gm.eventManager.AddToList(ChangeEditorState);
        LevelEditorBtn.onClick.AddListener(delegate () { gm.eventManager.ExecuteChangeEditorState(); });
    }

    public void ChangeEditorState()
    {
        ColorBlock cb = LevelEditorBtn.colors;
        switch (editorState)
        {
            case EditorState.Editing:
                cb.pressedColor = cb.highlightedColor = cb.normalColor = NONEDITINGCOLOR;
                editorState = EditorState.NonEditing;
                break;
            case EditorState.NonEditing:
                cb.pressedColor = cb.highlightedColor = cb.normalColor = EDITINGCOLOR;
                editorState = EditorState.Editing;
                break;
        }
        LevelEditorBtn.colors = cb;
    }

    public void SetDefaultStart()
    {
        ColorBlock cb = LevelEditorBtn.colors;
        if (gm.gameState != GameManager.GameState.Editor)
        {
            cb.pressedColor = cb.highlightedColor = cb.normalColor = NONEDITINGCOLOR;
            editorState = EditorState.NonEditing;
        }
        else
        {
            cb.pressedColor = cb.highlightedColor = cb.normalColor = EDITINGCOLOR;
            editorState = EditorState.Editing;
        }
        LevelEditorBtn.colors = cb;
    }
}