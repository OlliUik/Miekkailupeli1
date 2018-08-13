﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDeathState : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameHandler.instance.BattleEnded = true;
        Debug.Log("Battle ended");
    }
}
