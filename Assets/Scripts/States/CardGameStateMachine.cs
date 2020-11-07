using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameStateMachine : StateMachine
{
    [SerializeField] GameManager manager;
    [SerializeField] GameUI ui;

    protected override void Initialize(State targetState)
    {
        ((CardGameState)targetState).Initialize(this, manager, ui);
    }
}
