using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public abstract class MasterPage : MonoBehaviour
{
    [SerializeField]
    protected EPages pageView;

    protected Animator animatorController;

    protected CanvasManager canvasManager;

    protected const string NEXTPAGEVIEW = "OpenPageView";
    protected const string BACKPAGEVIEW = "BackPageView";
    protected const string CLOSEPAGEVIEW = "ClosePageView";
    protected const string CURRENTBACKPAGEVIEW = "CurrentBackPageView";

    protected virtual void Awake()
    {
        canvasManager = CanvasManager.Instance;
        AddPage();
        animatorController = GetComponent<Animator>();
        animatorController.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/TransitionController");

        //this.gameObject.SetActive(false);
    }

    // Use this for initialization
    protected virtual void Start()
    {
        //canvasManager = CanvasManager.Instance;
        //AddPage();
        this.gameObject.SetActive(false);
    }

    public void PageViewAnimation(EPageViewAnimation nextPageViewAnimation)
    {

        switch (nextPageViewAnimation)
        {
            case EPageViewAnimation.Open:
                CallAnimatorTrigger(NEXTPAGEVIEW);
                break;

            case EPageViewAnimation.Back:
                CallAnimatorTrigger(BACKPAGEVIEW);
                break;

            case EPageViewAnimation.Close:
                CallAnimatorTrigger(CLOSEPAGEVIEW);
                break;

            case EPageViewAnimation.CurrentBack:
                CallAnimatorTrigger(CURRENTBACKPAGEVIEW);
                break;

            default:
                break;
        }

    }

    /// <summary>
    /// Used to ref the back button on the page
    /// </summary>
    /// <param name="backBtn">By parameter especify which button is</param>
    protected void BackButtonReference(Button backBtn)
    {
        backBtn.onClick.AddListener(delegate { canvasManager.BackPage(); });
        backBtn.onClick.AddListener(delegate { SoundManager.Instance.PlayUiButtonSound(); });
    }

    protected void AddPage()
    {
        canvasManager.AddToPageDictionary(pageView, this);
    }

    protected void ButtonToPageRef(Button button, EPages page)
    {
        button.onClick.AddListener(delegate { canvasManager.NextPage(page); });
        button.onClick.AddListener(delegate { SoundManager.Instance.PlayUiButtonSound(); });
    }

    private void CallAnimatorTrigger(string animation)
    {
        animatorController.SetTrigger(animation);
    }

    #region BeginAnimations
    /// <summary>
    /// Left To Center
    /// </summary>
    protected virtual void BeginNextAnimation()
    {
        this.gameObject.SetActive(true);
        LockTouches.Instance.EnableTouches(true);
    }

    /// <summary>
    /// Right To Center
    /// </summary>
    protected virtual void BeginBackAnimation()
    {
        this.gameObject.SetActive(true);
        LockTouches.Instance.EnableTouches(true);
    }

    /// <summary>
    /// Center To Right
    /// </summary>
    protected virtual void BeginCloseAnimation()
    {
        this.gameObject.SetActive(true);
        LockTouches.Instance.EnableTouches(true);
    }

    /// <summary>
    /// Center To Left
    /// </summary>
    protected virtual void BeginCurrentBackAnimation()
    {
        this.gameObject.SetActive(true);
        LockTouches.Instance.EnableTouches(true);
    }
    #endregion

    #region EndAnimations
    /// <summary>
    /// Left To Center
    /// </summary>
    protected virtual void EndNextAnimation()
    {
        LockTouches.Instance.EnableTouches(false);
    }

    /// <summary>
    /// Right To Center
    /// </summary>
    protected virtual void EndBackAnimation()
    {
        LockTouches.Instance.EnableTouches(false);
    }

    /// <summary>
    /// Center To Right
    /// </summary>
    protected virtual void EndCloseAnimation()
    {
        this.gameObject.SetActive(false);
        LockTouches.Instance.EnableTouches(false);
    }

    /// <summary>
    /// Center To Left
    /// </summary>
    protected virtual void EndCurrentBackAnimation()
    {
        this.gameObject.SetActive(false);
        LockTouches.Instance.EnableTouches(false);
    }
    #endregion

    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {

    }
}
