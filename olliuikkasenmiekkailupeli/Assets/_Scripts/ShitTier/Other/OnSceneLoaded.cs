﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneLoaded : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();
    int modelIndex;
    public GameObject fade;
    public float timer = 3;
    float reset;
    bool timerStarted;

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    public void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        reset = timer;

        fade.GetComponent<Animator>().Play("FadeOut");

        for (int i = 0; i < players.Count; i++)
        {
            players[i].gameObject.GetComponent<AlternativeMovement5>().enabled = false;
        }


        for (int i = 0; i < players.Count; i++)
        { 
            if(players[i].name == "P1") // or tag
            {
                players[i].GetComponent<ChooseIngameModel>().ChooseModel(GameHandler.instance.player1Model);
            }
            else if(players[i].name == "P2")
            {
                players[i].GetComponent<ChooseIngameModel>().ChooseModel(GameHandler.instance.player2Model);
            }
        }

        InputManager.IM.SetCorrectInputs();
        timerStarted = true;
    }

    void Update()
    {
        if(timerStarted)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {

                Debug.Log("FIGHT!!");

                for (int i = 0; i < players.Count; i++)
                {
                    players[i].gameObject.GetComponent<AlternativeMovement5>().enabled = true;
                }

                timer = reset;
                timerStarted = false;

                GameHandler.instance.battleStarted = true;
            }
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

}