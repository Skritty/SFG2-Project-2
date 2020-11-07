using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameState : State
{
    protected CardGameStateMachine stateMachine;
    protected GameManager manager;
    protected GameUI ui;

    public void Initialize(CardGameStateMachine _stateMachine, GameManager _manager, GameUI _ui)
    {
        stateMachine = _stateMachine;
        manager = _manager;
        ui = _ui;
    }
}
