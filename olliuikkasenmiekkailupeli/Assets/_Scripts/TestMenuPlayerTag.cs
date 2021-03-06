﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMenuPlayerTag : MonoBehaviour
{

    PlayerInfo info;

    void Start()
    {
        info = GetComponent<PlayerInfo>();
    }

    void Update()
    {
        //Huom! Kirjoita Tag():iin vasemman ja oikean pelaajan objektien nimet
        Tag();
    }

    void Tag()
    {
        if (InputManager.IM.isLeftP1 && this.gameObject.name == "L") //tähän vasemman nimi
        {
            this.gameObject.tag = "Player 1";
            info.playerIndex = 1;
            info.SetInputs(InputManager.IM.P1_Hor, InputManager.IM.P1_Ver);
        }

        if (InputManager.IM.isRightP1 && this.gameObject.name == "R") //tähän oikean nimi
        {
            this.gameObject.tag = "Player 1";
            info.playerIndex = 1;
            info.SetInputs(InputManager.IM.P1_Hor, InputManager.IM.P1_Ver);
        }

        if (InputManager.IM.isLeftP2 && this.gameObject.name == "L") //tähän vasemman nimi
        {
            this.gameObject.tag = "Player 2";
            info.playerIndex = 2;
            info.SetInputs(InputManager.IM.P2_Hor, InputManager.IM.P2_Ver);
        }

        if (InputManager.IM.isRightP2 && this.gameObject.name == "R") //tähän oikean nimi
        {
            this.gameObject.tag = "Player 2";
            info.playerIndex = 2;
            info.SetInputs(InputManager.IM.P2_Hor, InputManager.IM.P2_Ver);
        }
    }
}