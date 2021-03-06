﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    public AudioClip menuTheme;
    public AudioClip gameTheme;

    [SerializeField]
    string sceneName;

    void Start()
    {
        OnLevelWasLoaded(1);
    }

    void OnLevelWasLoaded(int sceneIndex)
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != sceneName)
        {
            sceneName = newSceneName;
            Invoke("PlayMusic", .2f);
        }
    }

    public void PlayMusic()
    {
        AudioClip clipToPlay = null;

        if (sceneName == "MenuScene")
        {
            clipToPlay = menuTheme;
        }
        else if(sceneName == "GameScene")
        {
            clipToPlay = gameTheme;
        }

        if (clipToPlay != null)
        {
            AudioManager.instance.PlayMusic(clipToPlay, 2);
        }
    }

}
