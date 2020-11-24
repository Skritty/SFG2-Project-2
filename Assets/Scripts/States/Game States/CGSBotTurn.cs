using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGSBotTurn : CardGameState
{
    public class CardPlayChoice
    {
        // Can be improved with more information to make a more accurate judgement.
        public Tile tile;
        public bool chosen = false;
        int choiceRating = -1;

        public CardPlayChoice(Tile t)
        {
            tile = t;
        }
    }

    public class CardPlayData
    {
        public Construction construction;
        public Tile playTile;
        public int cardRating = 0;

        public CardPlayData(Construction c, Tile t, int r)
        {
            construction = c;
            playTile = t;
            cardRating = r;
        }
    }

    List<CardPlayChoice> choices = new List<CardPlayChoice>();
    List<Construction> toRemove = new List<Construction>();

    public override void Enter()
    {
        GameManager.manager.UIManager.ShowButton();
        GameManager.manager.audioManager.PlayAudioClip(GameManager.manager.audioManager.settings.TurnChange, Vector3.zero);

        manager.currentPlayer = 1;
        ResetTurnStuff();
        DrawCards(GameManager.CurrentPlayer);
        GameManager.CurrentPlayer.powerGridRoot.TransferPower(GameManager.CurrentPlayer.powerGridRoot.Data.OutputRate);
        ActivateMachines();
        ChooseAndPlay();

        ui.playerEndTurnButtonText.text = "Bot Thinking...";
        ui.playerEndTurnButton.interactable = false;
        ui.UpdateUI();
    }

    public override void Tick()
    {
        if (GameManager.CurrentPlayer.coreCurrentPower <= 0)
        {
            stateMachine.ChangeState<CGSWin>();
        }
        else if (GameManager.CurrentPlayer.obelistPowerCurrent >= GameManager.CurrentPlayer.ObeliskPowerToWin)
        {
            stateMachine.ChangeState<CGSLose>();
        }
        else
        {
            stateMachine.ChangeState<CGSPlayerTurn>();
        }
    }

    public override void Exit()
    {
        GameManager.manager.UIManager.HideButton();
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
        if (card != null)
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
    #endregion

    #region AI

    private void PlaceCard(CardUI card, Tile tile)
    {
        // Acceptable to place?
        if (tile == null || card.info.Data.BoardPiece == null || (card.info.Data.CardPlayCost + GameManager.CurrentPlayer.cardsPlayed > GameManager.CurrentPlayer.cardsPlayedPerTurn && (GameManager.CurrentPlayer.extraCardsPlayed < 1 || tile.buildsRemaining == 0))
            || !GameManager.manager.board.TileInPlayerRange(GameManager.manager.currentPlayer, tile, GameManager.CurrentPlayer.interactionRange))
        {
            //card.ResetTransform();
            Debug.LogError("Failed to Build");
            return;
        }

        // Place Tracks
        if (card.info.Data.GetType() == typeof(Tracks) && tile.rail == null && (tile.contains == null || tile.contains.Data is IRailCompatable))
        {
            card.info.Build(tile);
            card.info.ownerIndex = GameManager.manager.currentPlayer;
            card.info.tile = tile;
            toRemove.Add(card.info);
            card.gameObject.SetActive(false);
            card.ResetTransform();
            GameManager.CurrentPlayer.cardsPlayed += card.info.Data.CardPlayCost;
            GameManager.manager.board.LowerBuildsRemaining(card.info.Data.CardPlayCost);
            card = null;
            ui.UpdateUI();
        }
        // Place non-tracks
        else if (tile.contains == null && (tile.rail == null || card.info.Data is IRailCompatable))
        {
            card.info.Build(tile);
            card.info.ownerIndex = GameManager.manager.currentPlayer;
            card.info.tile = tile;
            toRemove.Add(card.info);
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
            //card.ResetTransform();
            //Debug.LogError("Failed to Build "+card.info.Data.name+". Reason: "+ tile.contains.Data.name +" at "+tile.ToString());
            return;
        }
    }
    private void RemoveCardFromHand(Player p, Construction card)
    {
        p.hand.Remove(card);
        card.card.gameObject.SetActive(false);
    }

    private void ChooseAndPlay()
    {
        // Play all free cards
        foreach(Construction card in GameManager.CurrentPlayer.hand)
        {
            choices.Clear();
            // These choices are potential tile placement spots
            foreach (Tile t in GameManager.manager.board.GetPoweredTilesInPlayerRange(GameManager.manager.currentPlayer))
            {
                choices.Add(new CardPlayChoice(t));
            }

            int bestScore = -99;
            Tile selectedTile = null;
            
            if (card.Data.CardPlayCost == 0)
            {
                foreach (CardPlayChoice option in choices)
                {
                    if (card.Data.CalculateChoiceRating(option.tile, card) > bestScore)
                    {
                        bestScore = card.Data.CalculateChoiceRating(option.tile, card);
                        selectedTile = option.tile;
                    }
                }
                PlayAndConnect(new CardPlayData(card, selectedTile, card.Data.CardRating));
            }
        }

        int breakpoint = 0;
        //Play cost cards until there are no more places to play
        while (GameManager.manager.board.HasBuildSpot() && breakpoint < 20)
        {
            CardPlayData bestCostCard = null;
            foreach (Construction card in GameManager.CurrentPlayer.hand)
            {
                breakpoint++;
                choices.Clear();
                // These choices are potential tile placement spots
                foreach (Tile t in GameManager.manager.board.GetPoweredTilesInPlayerRange(GameManager.manager.currentPlayer))
                {
                    choices.Add(new CardPlayChoice(t));
                }

                int bestScore = -99;
                Tile selectedTile = null;
                
                if (card.Data.CardPlayCost != 0)
                {
                    if (bestCostCard != null && bestCostCard.cardRating > card.Data.CardRating) continue;
                    foreach (CardPlayChoice option in choices)
                    {
                        if (card.Data.CalculateChoiceRating(option.tile, card) > bestScore)
                        {
                            bestScore = card.Data.CalculateChoiceRating(option.tile, card);
                            selectedTile = option.tile;
                        }
                    }
                    bestCostCard = new CardPlayData(card, selectedTile, card.Data.CardRating);
                }
            }

            if (bestCostCard != null) PlayAndConnect(bestCostCard);
            else break;
            foreach (Construction c in toRemove) GameManager.CurrentPlayer.hand.Remove(c);
        }
        
        Construction source = FindFirst(GameManager.CurrentPlayer.powerGridRoot, GameManager.CurrentPlayer.ownedOnBoard[0].tile);
        if(source != null && GameManager.CurrentPlayer.ownedOnBoard[0].inputConnections == 0) CommandInvoker.ExecuteCommand(new Connect(source, GameManager.CurrentPlayer.ownedOnBoard[0], source.color));
    }

    private void PlayAndConnect(CardPlayData toPlay)
    {
        PlaceCard(toPlay.construction.card, toPlay.playTile);
        if (toPlay.construction.obj == null || toPlay.construction.Data is Tracks) return;
        Construction source = FindFirst(GameManager.CurrentPlayer.powerGridRoot, toPlay.playTile);
        CommandInvoker.ExecuteCommand(new Connect(source, toPlay.construction, source.color));
    }

    private Construction FindFirst(Construction current, Tile target)
    {
        Construction success = null;
        if (GameManager.manager.board.TileInConstructionRange(current, target)) return current;
        else foreach (WireConnection wc in current.connections) if ((success = FindFirst(wc.target, target)) != null) break;
        return success;
    }
    #endregion
}
