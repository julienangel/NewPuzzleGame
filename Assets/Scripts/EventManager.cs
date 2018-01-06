using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager
{
    private MonoBehaviour mono;
    private List<Action> ChangeEditorStateList;

    public EventManager(MonoBehaviour mono)
    {
        this.mono = mono;
        ChangeEditorStateList = new List<Action>();
    }

    public void ExecuteChangeEditorState()
    {
        int changeEditorStateListCount = ChangeEditorStateList.Count;
        for (int i = 0; i < changeEditorStateListCount; i++)
        {
            ChangeEditorStateList[i].Invoke();
        }
    }

    public void AddToList(Action action)
    {
        ChangeEditorStateList.Add(action);
    }

    public void RemoveFromList(Action action)
    {
        ChangeEditorStateList.Remove(action);
    }
}
