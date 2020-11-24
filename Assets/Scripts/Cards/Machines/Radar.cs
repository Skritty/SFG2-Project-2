using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Radar", menuName = "Constructions/Machines/Radar")]
public class Radar : ConstructionData, IRailCompatable
{
    public override void DoTurn(Construction target)
    {
        foreach(Tile t in GameManager.manager.board.GetConstructionTilesInRadius(target.tile, EffectRange))
        {
            if (!t.contains.accessProtected)
            {
                t.contains.accessProtected = true;
            }
        }
    }

    public void Move(Transform target)
    {
        throw new System.NotImplementedException();
    }

    public override int CalculateChoiceRating(Tile tile, Construction construction)
    {
        List<Construction> nearbyAllyConstructions = new List<Construction>();
        List<Construction> nearbyEnemyConstructions = new List<Construction>();

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

        return -nearbyAllyConstructions.Count + nearbyEnemyConstructions.Count;
    }
}
