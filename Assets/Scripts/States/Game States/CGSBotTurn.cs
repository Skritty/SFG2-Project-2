using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGSBotTurn : CardGameState
{
    public override void Enter()
    {
        manager.currentPlayer = 1;//(manager.currentPlayer + 1) % 2;
        DrawCards(manager.players[manager.currentPlayer]);
        ActivateMachines();
        ui.playerEndTurnButtonText.text = "Bot Thinking...";
        ui.playerEndTurnButton.interactable = false;
        ui.UpdateUI();
    }
    public override void Tick()
    {
        stateMachine.ChangeState<CGSPlayerTurn>();
    }

    private void DrawCards(Player p)
    {
        Debug.Log(p.gameObject.name);
        p.hand.Add(p.machineDeck.Draw());
        p.hand.Add(p.infrastructureDeck.Draw());
    }

    private void ActivateMachines()
    {
        foreach (Tile[] tiles in manager.board.board)
        {
            foreach (Tile t in tiles)
            {
                foreach (Construction c in t.contains)
                {
                    if (c == null) continue;
                    c.Data.ConsumePower(c);
                    c.Data.DoTurn(c);
                }
            }
        }
    }
}
