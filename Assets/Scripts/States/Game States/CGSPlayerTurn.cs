using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGSPlayerTurn : CardGameState
{
    int cardsPlayed = 0;
    int actionsDone = 0;

    int bonusCardPlays = 0;
    int bonusActions = 0;

    public override void Enter()
    {
        manager.currentPlayer = 0;//(manager.currentPlayer + 1) % 2;
        cardsPlayed = 0;
        actionsDone = 0;
        DrawCards(manager.players[manager.currentPlayer]);
        ActivateMachines();
        ui.playerEndTurnButtonText.text = "End Turn";
        ui.playerEndTurnButton.interactable = true;
        ui.UpdateUI();

        GameInput.OnCardDropped += PlaceCard;
        GameInput.WhileCardHeld += CardFollow;
        GameInput.OnTileSelect += ShowConstructionMenu;
        GameInput.OnTileDeselect += HideConstructionMenu;
        GameInput.OnClickCommand += DoAction;
    }

    public override void Tick()
    {
        
    }

    public override void Exit()
    {
        HideConstructionMenu(null);
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
                foreach(Construction c in t.contains)
                {
                    if (c == null) continue;
                    c.Data.ConsumePower(c);
                    c.Data.DoTurn(c);
                }
            }
        }
    }

    private void CardFollow(CardUI card, Tile tile)
    {
        card.Transform(Input.mousePosition, ui.settings.CardMoveScale);
    }

    private void PlaceCard(CardUI card, Tile tile)
    {
        if (tile != null
                    && (tile.contains.Count == 0
                    || (tile.contains[0].Data as IRailCompatable != null && card.info.Data.GetType() == typeof(Tracks))
                    || (card.info.Data as IRailCompatable != null && tile.contains[0].Data.GetType() == typeof(Tracks))
                    ))
        {
            if (cardsPlayed + card.info.Data.CardPlayCost > manager.players[manager.currentPlayer].cardsPlayedPerTurn)
            {
                card.ResetTransform();
                return;
            }
            card.info.Build(tile);
            manager.players[manager.currentPlayer].hand.Remove(card.info);
            card.gameObject.SetActive(false);
            card.ResetTransform();
            cardsPlayed += card.info.Data.CardPlayCost;
            card = null;
            ui.UpdateUI();
        }
        else
        {
            card.ResetTransform();
        }
    }

    private void ShowConstructionMenu(Tile tile)
    {
        if (tile != null && tile.contains.Count > 0)
        {
            Construction c = tile.contains.Find(x => x != null && x.Data.GetType() != typeof(Tracks));
            if (c == null) return;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(tile.transform.position + ui.settings.RelPosFromTile);
            ui.selectInfo.transform.position = screenPos;
            ui.selectInfo.EnableElements(c.Data.SelectionUI);
            ui.selectInfo.gameObject.SetActive(true);
        }
    }

    private void HideConstructionMenu(Tile tile)
    {
        ui.selectInfo.gameObject.SetActive(false);
    }

    private void RemoveCardFromHand(Player p, Construction card)
    {
        p.hand.Remove(card);
        card.card.gameObject.SetActive(false);
    }

    private void DoAction(SelectUIElements command, Tile tile)
    {
        if (actionsDone + 1 > manager.players[manager.currentPlayer].actionsPerTurn) return;
        Construction c = tile.contains.Find(x => x != null && x.Data.GetType() != typeof(Tracks));
        switch (command)
        {
            case SelectUIElements.Connect:
                CommandInvoker.ExecuteCommand(new Connect(c, tile));
                break;
            case SelectUIElements.Move:
                break;
            case SelectUIElements.Rotate:
                break;
            case SelectUIElements.Toggle:
                CommandInvoker.ExecuteCommand(new Toggle(c, tile));
                break;
        }
    }
}
