using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pylon", menuName = "Constructions/Infrastructure/Pylon")]
public class Pylon : ConstructionData, IPowerTransferable
{
    public override void Connect(Construction self, Construction target, ColorType type)
    {
        base.Connect(self, target, type);
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

        if (GameManager.CurrentPlayer.ownedOnBoard[0].inputConnections == 0) return distanceToCore - distanceToObelisk;
        else return -nearbyAllyConstructions.Count;

    }
}
