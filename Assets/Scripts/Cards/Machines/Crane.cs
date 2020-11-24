using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crane", menuName = "Constructions/Machines/Crane")]
public class Crane : ConstructionData, IRailCompatable
{
    public override void DoTurn(Construction target)
    {
        if (GameManager.manager.currentPlayer == target.ownerIndex) return;
        GameManager.CurrentPlayer.extraCardsPlayed++;
        foreach(Tile t in GameManager.manager.board.GetAllTilesInRadius(target.tile, EffectRange))
        {
            t.buildsRemaining++;
        }
    }

    public override int CalculateChoiceRating(Tile tile, Construction construction)
    {
        List<Construction> nearbyAllyConstructions = new List<Construction>();
        List<Construction> nearbyEnemyConstructions = new List<Construction>();
        int distanceToCore = Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[1].tile.x - tile.x) + Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[1].tile.y - tile.y);
        int distanceToObelisk = Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[0].tile.x - tile.x) + Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[0].tile.y - tile.y);

        if (tile.contains != null) return -100;
        foreach (Tile t in GameManager.manager.board.GetConstructionTilesInRadius(tile, construction.Data.EffectRange))
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

        return distanceToCore - nearbyAllyConstructions.Count;
    }
}

