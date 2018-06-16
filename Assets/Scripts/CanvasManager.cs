using System;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private List<MasterPage> m = new List<MasterPage>();

    private static CanvasManager instance;
    public static CanvasManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CanvasManager>();
            }
            return instance;
        }
    }

    public struct PreviousPageManager
    {
        private List<EPages> previousPageManager;

        public PreviousPageManager(EPages startingPage)
        {
            previousPageManager = new List<EPages>();
            AddPage(startingPage);
        }

        public EPages AddPage(EPages newPage)
        {
            previousPageManager.Add(newPage);
            return newPage;
        }

        public EPages RemoveLast()
        {
            previousPageManager.RemoveAt(previousPageManager.Count - 1);
            return previousPageManager[previousPageManager.Count - 1];
        }

        public EPages CurrentPage()
        {
            return previousPageManager[previousPageManager.Count - 1];
        }

        public EPages BeforeCurrentPage()
        {
            return previousPageManager[previousPageManager.Count - 2];
        }
    };

    private PreviousPageManager previousPageManager;

    private Dictionary<EPages, MasterPage> PagesManager = new Dictionary<EPages, MasterPage>();

    private SoundManager soundManager;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        previousPageManager = new PreviousPageManager(EPages.MainMenu);

        soundManager = SoundManager.Create();

        /// Activates the master page objects to add to the dictionary, and then turn them off
        int length = m.Count;
        for (int i = 0; i < length; i++)
        {
            m[i].gameObject.SetActive(true);
        }
    }

    // Use this for initialization
    void Start()
    {
        Invoke("GoToMainPage", 0);
    }

    public void AddToPageDictionary(EPages page, MasterPage pageClass)
    {
        PagesManager.Add(page, pageClass);
    }

    public void NextPage(EPages nextPage)
    {
        previousPageManager.AddPage(nextPage);

        MasterPage currentMasterPage = PagesManager[previousPageManager.CurrentPage()];
        MasterPage previousMasterPage = PagesManager[previousPageManager.BeforeCurrentPage()];

        previousMasterPage.gameObject.SetActive(true);
        currentMasterPage.gameObject.SetActive(true);

        previousMasterPage.PageViewAnimation(EPageViewAnimation.Close);
        currentMasterPage.PageViewAnimation(EPageViewAnimation.Open);
    }

    public void BackPage()
    {
        MasterPage currentMasterPage = PagesManager[previousPageManager.BeforeCurrentPage()];
        MasterPage previousMasterPage = PagesManager[previousPageManager.CurrentPage()];

        previousMasterPage.gameObject.SetActive(true);
        currentMasterPage.gameObject.SetActive(true);

        previousMasterPage.PageViewAnimation(EPageViewAnimation.CurrentBack);
        currentMasterPage.PageViewAnimation(EPageViewAnimation.Back);

        previousPageManager.RemoveLast();
    }
}
