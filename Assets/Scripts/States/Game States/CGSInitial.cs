using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGSInitial : CardGameState
{
    public override void Enter()
    {
        foreach(Player p in manager.players)
        {
            CreateDecks(p);
            InitializePlayer(p);
        }
    }

    public override void Exit()
    {
        
    }

    public override void Tick()
    {
        stateMachine.ChangeState<CGSPlayerTurn>();
    }

    private void CreateDecks(Player p)
    {
        foreach (ConstructionData data in p.machineDeckInitial)
        {
            p.machineDeck.Add(new Construction(data, p.playerIndex, p.defaultCard));
        }
        p.machineDeck.Shuffle();

        foreach (ConstructionData data in p.infrastructureDeckInitial)
        {
            p.infrastructureDeck.Add(new Construction(data, p.playerIndex, p.defaultCard));
        }
        p.infrastructureDeck.Shuffle();
    }

    private void InitializePlayer(Player p)
    {
        for (float i = 0; i < p.startingMachineCards; i++)
        {
            p.hand.Add(p.machineDeck.Draw());
        }
        for (float i = 0; i < p.startingInfrastructureCards; i++)
        {
            p.hand.Add(p.infrastructureDeck.Draw());
        }

        ui.UpdateUI();
    }
}
