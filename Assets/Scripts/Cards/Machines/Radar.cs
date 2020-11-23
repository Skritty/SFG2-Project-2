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
}
