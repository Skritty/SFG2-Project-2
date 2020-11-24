using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGSInitial : CardGameState
{
    public override void Enter()
    {
        GameManager.manager.audioManager.SwitchSongs(GameManager.manager.audioManager.settings.PlayerMusic);
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

        foreach (ConstructionData data in p.discardInitial)
        {
            p.discard.Add(new Construction(data, p.playerIndex, p.defaultCard));
        }
        p.discard.Shuffle();
    }

    private void InitializePlayer(Player p)
    {
        for (float i = 0; i < p.startingMachineCards; i++)
        {
            Construction card = p.machineDeck.Draw();
            if (card != null) p.hand.Add(card);
        }
        for (float i = 0; i < p.startingInfrastructureCards; i++)
        {
            Construction card = p.infrastructureDeck.Draw();
            if (card != null) p.hand.Add(card);
        }

        ui.UpdateUI();
    }
}
