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

        Construction construction;
        List<Construction> nearbyAllyConstructions = new List<Construction>();
        List<Construction> nearbyEnemyConstructions = new List<Construction>();
        int distanceToCore = 0;
        int distanceToObelisk = 0;
        

        public CardPlayChoice(Tile t)
        {
            tile = t;
            distanceToCore = Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[1].tile.x - t.x) + Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[1].tile.y - t.y);
            distanceToObelisk = Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[0].tile.x - t.x) + Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[0].tile.y - t.y);
        }

        //TODO: Add a better AI by writing a function for each construction that takes in an improved "CardPlayChoice" class and returns a "rating" depending on how good that tile would be to play in.
        public int CalculateChoiceRating(Construction c)
        {
            if (tile.contains != null) return -100;
            foreach (Tile t in GameManager.manager.board.GetConstructionTilesInRadius(tile, c.Data.EffectRange))
            {
                if (t.contains.ownerIndex == GameManager.manager.currentPlayer)
                {
                    nearbyAllyConstructions.Add(t.contains);
                }
                else
                {
                    nearbyEnemyConstructions.Add(t.contains);
                }
            }

            return distanceToCore - distanceToObelisk - nearbyAllyConstructions.Count + nearbyEnemyConstructions.Count;
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
        if (GameManager.CurrentPlayer.obelistPowerCurrent >= GameManager.CurrentPlayer.ObeliskPowerToWin)
        {
            stateMachine.ChangeState<CGSLose>();
        }

        stateMachine.ChangeState<CGSPlayerTurn>();
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
        if (card != null) p.hand.Add(card);
        card = p.infrastructureDeck.Draw();
        if (card != null) p.hand.Add(card);
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
        Debug.Log(card.info.Data.name);
        // Acceptable to place?
        if (tile == null || (card.info.Data.CardPlayCost + GameManager.CurrentPlayer.cardsPlayed > GameManager.CurrentPlayer.cardsPlayedPerTurn && (GameManager.CurrentPlayer.extraCardsPlayed < 1 || !tile.bonusBuilds))
            || !GameManager.manager.board.TileInPlayerRange(GameManager.manager.currentPlayer, tile, GameManager.CurrentPlayer.interactionRange))
        {
            //card.ResetTransform();
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
            card = null;
            ui.UpdateUI();
        }
        else
        {
            //card.ResetTransform();
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

        List<CardPlayData> toPlay = new List<CardPlayData>();
        CardPlayData bestCostCard = null;
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

            // If it's a 0 cost card, find the best place to play it and do so
            if (card.Data.CardPlayCost == 0)
            {
                foreach (CardPlayChoice option in choices)
                {
                    Debug.Log(option.CalculateChoiceRating(card));
                    if (option.CalculateChoiceRating(card) > bestScore)
                    {
                        bestScore = option.CalculateChoiceRating(card);
                        selectedTile = option.tile;
                    }
                }
                PlaceCard(card.card, selectedTile);
            }
            // If it is a 1 cost card, find out which has the highest play rating
            else
            {
                if (bestCostCard != null && bestCostCard.cardRating > card.Data.CardRating) continue;
                foreach (CardPlayChoice option in choices)
                {
                    if (option.CalculateChoiceRating(card) > bestScore)
                    {
                        bestScore = option.CalculateChoiceRating(card);
                        selectedTile = option.tile;
                    }
                }
                bestCostCard = new CardPlayData(card, selectedTile, card.Data.CardRating);
            }
        }
        if(bestCostCard != null) PlaceCard(bestCostCard.construction.card, bestCostCard.playTile);
        foreach(Construction c in toRemove) GameManager.CurrentPlayer.hand.Remove(c);
        //PlayCards(toPlay);
    }

    /*private void PlayCards(List<CardPlayData> toPlay)
    {
        foreach(CardPlayData data in toPlay)
        {
            Debug.Log("Bot is playing " + data.construction.Data.name + " at tile ("+data.playTile.x+","+data.playTile.y+")");
            PlaceCard(data.construction.card, data.playTile);
        }
    }*/
    #endregion
}
