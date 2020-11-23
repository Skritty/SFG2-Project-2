using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGSWin : CardGameState
{
    public override void Enter()
    {
        ui.winScreen.SetActive(true);
    }

    public override void Exit()
    {
        ui.winScreen.SetActive(false);
    }
}
