using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }
            return instance;
        }
    }

    AudioClip mainTheme;
    AudioClip uiTouch;

    public AudioSource audioSource;
    public AudioSource sounds;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        //LoadSoundtrackFromResources();
        audioSource.clip = mainTheme;
        audioSource.loop = true;
        audioSource.Play();
        sounds = this.gameObject.AddComponent<AudioSource>();
        //sounds.volume = gameManager.playerData.Sound;
    }

    public static SoundManager Create()
    {
        GameObject gameObject = new GameObject
        {
            name = "Sound Manager"
        };
        SoundManager soundManager = gameObject.AddComponent<SoundManager>();
        soundManager.gameObject.AddComponent<AudioSource>();
        return soundManager;
    }

    void LoadSoundtrackFromResources()
    {
        mainTheme = Resources.Load<AudioClip>("Soundtrack/MainTheme");
        uiTouch = Resources.Load<AudioClip>("Soundtrack/UITouch");
    }

    public void PlayUiButtonSound()
    {
        print("Carregou!");
    }
}
