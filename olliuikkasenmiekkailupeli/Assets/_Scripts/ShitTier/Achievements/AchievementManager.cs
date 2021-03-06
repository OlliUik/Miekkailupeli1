﻿using System.Linq;
using UnityEngine;
using System.Collections;
using Steamworks;
using System;

public class AchievementManager : MonoBehaviour
{
    public bool debuggings = false;

    public static AchievementManager instance = null;
    private bool unlockTest = false;

    public Achievement[] Achievements;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // deactivate this is no connection to steam
        if(!SteamManager.Initialized && !debuggings) 
        {
            gameObject.SetActive(false);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetProgressToAchievement("Master the blade", PlayerPrefs.GetInt("gamesPlayed"));
        if (debuggings)
            SetProgressToAchievement("Master the blade", 0);
    }

    private void Update()
    {

        if (debuggings)
        {
            if(!SteamManager.Initialized)
            {
                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    Debug.Log("lock");
                    DEBUG_LockAchievement("CompleteTutorial");
                }
            }
            else if(Input.GetKeyDown(KeyCode.Keypad0)) // DEBUGGING
            {
                Debug.Log("lock");
                DEBUG_LockAchievement("CompleteTutorial");
            }
        }

    }

    private Achievement GetAchievementByName(string achievementName)
    {
        return Achievements.FirstOrDefault(achievement => achievement.Name == achievementName);
    }

    private void AchievementEarned(string achievementName)
    {
        Achievement achievement = GetAchievementByName(achievementName);

        if(!SteamManager.Initialized)
        {
            Debug.Log("Steam not initialized...trying to get achievement: " + achievement.Name);
        }
        else
        {
            // unlock steam achievement
            TestSteamAchievement(achievement.ID);
            if(!unlockTest)
            {
                SteamUserStats.SetAchievement(achievementName);
                SteamUserStats.StoreStats();
                Debug.Log(achievement + " unlocked");
            }
            else
            {
                Debug.Log("achievement already achieved?");
            }
        }
    }

    private void TestSteamAchievement(string ID)
    {
        SteamUserStats.GetAchievement(ID, out unlockTest);

        if (!unlockTest)
            Debug.Log("Achievement not unlocked yet");
        else
            Debug.Log("Achievement is unlocked");
    }

    public void AddProgressToAchievement(string achievementName, float progressAmount)
    {
        Achievement achievement = GetAchievementByName(achievementName);
        if (achievement == null)
        {
            Debug.Log("AddProgressToAchievement() - Trying to add progress to an achievemnet that doesn't exist: " + achievementName);
            return;
        }

        if (achievement.AddProgress(progressAmount))
        {
            AchievementEarned(achievementName);
            Debug.Log(achievementName + " achievement earned");
        }
    }

    public void SetProgressToAchievement(string achievementName, float newProgress)
    {
        Achievement achievement = GetAchievementByName(achievementName);
        if (achievement == null)
        {
            Debug.Log("SetProgressToAchievement() - Trying to add progress to an achievemnet that doesn't exist: " + achievementName);
            return;
        }

        if (achievement.SetProgress(newProgress))
        {
            AchievementEarned(achievementName);
        }
    }

    public void DEBUG_LockAchievement(string achievementName)
    {
        var achievement = GetAchievementByName(achievementName);

        if(!SteamManager.Initialized)
        {
            Debug.Log("steam not initialized");
            achievement.LockAchievement();
        }
    }
}
