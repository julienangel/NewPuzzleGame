using UnityEngine;
using UnityEngine.UI;

public class LockTouches : MonoBehaviour {

    Image image;

    private static LockTouches instance;
    public static LockTouches Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LockTouches>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        image = GetComponent<Image>();
    }

    public void EnableTouches(bool active)
    {
        image.raycastTarget = active;
    }
}
