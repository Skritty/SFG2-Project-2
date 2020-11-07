using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGSMainMenu : CardGameState
{
    public override void Enter()
    {
        ui.mainMenu.SetActive(true);
    }

    public override void Exit()
    {
        ui.mainMenu.SetActive(false);
    }
}
