using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGSPlayerTurn : CardGameState
{
    Tile selectedTile = null;
    SelectUIElements storedCommand;
    Construction storedConstruction;

    public override void Enter()
    {
        GameManager.manager.audioManager.PlayAudioClip(GameManager.manager.audioManager.settings.TurnChange, Vector3.zero);

        manager.currentPlayer = 0;
        ResetTurnStuff();
        DrawCards(GameManager.CurrentPlayer);
        GameManager.CurrentPlayer.powerGridRoot.TransferPower(GameManager.CurrentPlayer.powerGridRoot.Data.OutputRate);
        ActivateMachines();

        ui.playerEndTurnButtonText.text = "End Turn";
        ui.playerEndTurnButton.interactable = true;
        ui.UpdateUI();

        Subscribe();
    }

    public override void Tick()
    {
        if (GameManager.CurrentPlayer.coreCurrentPower <= 0)
        {
            stateMachine.ChangeState<CGSLose>();
        }
        if (GameManager.CurrentPlayer.obelistPowerCurrent >= GameManager.CurrentPlayer.ObeliskPowerToWin)
        {
            stateMachine.ChangeState<CGSWin>();
        }
    }

    public override void Exit()
    {
        HideConstructionMenu(null);
        Unsubscribe();
    }

    #region Turn Initialization
    private void ResetTurnStuff()
    {
        GameManager.CurrentPlayer.cardsPlayed = 0;
        GameManager.CurrentPlayer.actionsDone = 0;
        GameManager.CurrentPlayer.extraActionsDone = 0;
        GameManager.CurrentPlayer.extraCardsPlayed = 0;
        GameManager.manager.board.ResetTileBuffs();
    }

    private void DrawCards(Player p)
    {
        Construction card = p.machineDeck.Draw();
        if(card != null)
        {
            p.hand.Add(card);
            GameManager.manager.audioManager.PlayAudioClip(GameManager.manager.audioManager.settings.CardDraw, Vector3.zero);
        }
        card = p.infrastructureDeck.Draw();
        if (card != null)
        {
            p.hand.Add(card);
            GameManager.manager.audioManager.PlayAudioClip(GameManager.manager.audioManager.settings.CardDraw, Vector3.zero);
        }
    }

    private void ActivateMachines()
    {
        foreach (Tile[] tiles in manager.board.board)
        {
            foreach (Tile t in tiles)
            {
                Construction c = t.contains;
                if (c == null || !c.powered) continue;
                c.Data.DoTurn(c);
            }
        }
    }

    private void Subscribe()
    {
        GameInput.OnCardDropped += ResetCardPlaceRange;
        GameInput.OnCardDropped += PlaceCard;
        GameInput.OnCardPickup += CardPlaceRange;
        GameInput.WhileCardHeld += CardFollow;
        GameInput.OnTileSelect += ShowConstructionMenu;
        GameInput.OnTileDeselect += HideConstructionMenu;
        GameInput.OnTileSelect += ShowSelectionRange;
        GameInput.OnTileDeselect += HideSelectionRange;
        GameInput.OnClickCommand += DoAction;
    }
    private void Unsubscribe()
    {
        GameInput.OnCardDropped -= ResetCardPlaceRange;
        GameInput.OnCardDropped -= PlaceCard;
        GameInput.OnCardPickup -= CardPlaceRange;
        GameInput.WhileCardHeld -= CardFollow;
        GameInput.OnTileSelect -= ShowConstructionMenu;
        GameInput.OnTileDeselect -= HideConstructionMenu;
        GameInput.OnTileSelect -= ShowSelectionRange;
        GameInput.OnTileDeselect -= HideSelectionRange;
        GameInput.OnClickCommand -= DoAction;
    }
    #endregion

    #region UI Interaction and Feedback
    private void CardFollow(CardUI card, Tile tile)
    {
        if (!GameManager.CurrentPlayer.hand.Contains(card.info)) return;
        card.Transform(Input.mousePosition, ui.settings.CardMoveScale);
    }

    private void PlaceCard(CardUI card, Tile tile)
    {
        // Acceptable to place?
        if (tile == null || card.info.Data.BoardPiece == null || (card.info.Data.CardPlayCost + GameManager.CurrentPlayer.cardsPlayed > GameManager.CurrentPlayer.cardsPlayedPerTurn && (GameManager.CurrentPlayer.extraCardsPlayed < 1 || tile.buildsRemaining == 0))
            || !GameManager.manager.board.TileInPlayerRange(GameManager.manager.currentPlayer, tile, GameManager.CurrentPlayer.interactionRange))
        {
            card.ResetTransform();
            return;
        }

        // Place Tracks
        if(card.info.Data.GetType() == typeof(Tracks) && tile.rail == null && (tile.contains == null || tile.contains.Data is IRailCompatable))
        {
            card.info.Build(tile);
            card.info.ownerIndex = GameManager.manager.currentPlayer;
            card.info.tile = tile;
            GameManager.CurrentPlayer.hand.Remove(card.info);
            card.gameObject.SetActive(false);
            card.ResetTransform();
            GameManager.CurrentPlayer.cardsPlayed += card.info.Data.CardPlayCost;
            GameManager.manager.board.LowerBuildsRemaining(card.info.Data.CardPlayCost);
            card = null;
            ui.UpdateUI();
        }
        // Place non-tracks
        else if(tile.contains == null && (tile.rail == null || card.info.Data is IRailCompatable))
        {
            card.info.Build(tile);
            card.info.ownerIndex = GameManager.manager.currentPlayer;
            card.info.tile = tile;
            GameManager.CurrentPlayer.hand.Remove(card.info);
            GameManager.CurrentPlayer.ownedOnBoard.Add(card.info);
            card.gameObject.SetActive(false);
            card.ResetTransform();
            GameManager.CurrentPlayer.cardsPlayed += card.info.Data.CardPlayCost;
            GameManager.manager.board.LowerBuildsRemaining(card.info.Data.CardPlayCost);
            card = null;
            ui.UpdateUI();
        }
        else
        {
            card.ResetTransform();
            return;
        }
    }
    private void RemoveCardFromHand(Player p, Construction card)
    {
        p.hand.Remove(card);
        card.card.gameObject.SetActive(false);
    }

    private void CardPlaceRange(CardUI card, Tile tile)
    {
        if (!GameManager.CurrentPlayer.hand.Contains(card.info)) return;
        foreach (Tile t in GameManager.manager.board.GetTilesInPlayerRange(GameManager.manager.currentPlayer, GameManager.CurrentPlayer.interactionRange))
        {
            if (t.buildsRemaining - card.info.Data.CardPlayCost >= 0) t.SetMaterial(GameManager.manager.UIManager.settings.InteractionRange);
        }
    }
    private void ResetCardPlaceRange(CardUI card, Tile tile)
    {
        foreach (Tile t in GameManager.manager.board.GetTilesInPlayerRange(GameManager.manager.currentPlayer, GameManager.CurrentPlayer.interactionRange))
        {
            t.RemoveMaterial(GameManager.manager.UIManager.settings.InteractionRange);
        }
    }

    private void ShowConstructionMenu(Tile tile)
    {
        if (tile != null && tile.contains != null && tile.contains.ownerIndex == GameManager.manager.currentPlayer)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(tile.transform.position + ui.settings.RelPosFromTile);
            ui.selectInfo.transform.position = screenPos;
            ui.selectInfo.EnableElements(tile.contains.Data.SelectionUI, tile.contains.color, tile);
            ui.selectInfo.gameObject.SetActive(true);
            selectedTile = tile;
        }
    }
    private void HideConstructionMenu(Tile tile)
    {
        ui.selectInfo.gameObject.SetActive(false);
    }

    private void ShowSelectionRange(Tile tile)
    {
        if (tile.contains != null)
            foreach (Tile t in GameManager.manager.board.GetAllTilesInRadius(tile, tile.contains.Data.EffectRange))
                 t.SetMaterial(GameManager.manager.UIManager.settings.SelectAoE);
    }
    private void HideSelectionRange(Tile tile)
    {
        if (tile != null && tile.contains != null)
            foreach (Tile t in GameManager.manager.board.GetAllTilesInRadius(tile, tile.contains.Data.EffectRange)) t.RemoveMaterial(GameManager.manager.UIManager.settings.SelectAoE);
    }
    #endregion

    private void DoAction(SelectUIElements command, ColorType color, Tile tile)
    {
        if (GameManager.CurrentPlayer.actionsDone + 1 > GameManager.CurrentPlayer.actionsPerTurn) return;
        switch (command)
        {
            case SelectUIElements.Connect:
                if (tile.contains.connections.Count >= tile.contains.Data.MaxOutputConnections) break;
                GameManager.manager.UIManager.ShowConnectionPoints(tile.contains, color);
                storedCommand = command;
                storedConstruction = tile.contains;
                break;
            case SelectUIElements.TargetActivate:
                GameManager.manager.UIManager.ShowTargetPoints(tile.contains);
                storedCommand = command;
                storedConstruction = tile.contains;
                break;
            case SelectUIElements.Rotate:
                GameManager.manager.UIManager.ShowDirections(tile);
                storedCommand = command;
                storedConstruction = tile.contains;
                break;
            case SelectUIElements.Move:
                //CommandInvoker.ExecuteCommand(new Move(tile.contains));
                break;
            case SelectUIElements.Activate:
                //CommandInvoker.ExecuteCommand(new Activate(tile.contains));
                break;
            case SelectUIElements.Toggle:
                CommandInvoker.ExecuteCommand(new Toggle(tile.contains));
                break;
            case SelectUIElements.Target:
                switch (storedCommand)
                {
                    case SelectUIElements.Connect:
                        CommandInvoker.ExecuteCommand(new Connect(storedConstruction, tile.contains, color));
                        break;
                    case SelectUIElements.TargetActivate:
                        CommandInvoker.ExecuteCommand(new TargetActivate(storedConstruction, tile.contains));
                        break;
                    case SelectUIElements.Rotate:
                        //CommandInvoker.ExecuteCommand(new Rotate(tile));
                        break;
                }
                GameManager.manager.UIManager.ClearSelections(tile);
                break;
        }
        HideConstructionMenu(tile);
    }
}
