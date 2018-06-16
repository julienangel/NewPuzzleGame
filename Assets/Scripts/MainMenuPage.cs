using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPage : MasterPage {

    private static MainMenuPage instance;
    public static MainMenuPage Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MainMenuPage>();
            }
            return instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }
}
