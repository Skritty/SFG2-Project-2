﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGSLose : CardGameState
{
    public override void Enter()
    {
        ui.loseScreen.SetActive(true);
    }

    public override void Exit()
    {
        ui.loseScreen.SetActive(false);
    }
}
