using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crane", menuName = "Constructions/Machines/Crane")]
public class Crane : ConstructionData, IRailCompatable
{
    public override void DoTurn(Construction target)
    {
        GameManager.CurrentPlayer.extraCardsPlayed++;
        foreach(Tile t in GameManager.manager.board.GetAllTilesInRadius(target.tile, EffectRange))
        {
            t.bonusBuilds = true;
        }
    }
}

