using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Radar", menuName = "Constructions/Machines/Radar")]
public class Radar : ConstructionData, IRailCompatable
{
    public override void DoTurn(Construction target)
    {
        base.DoTurn(target);
    }

    public void Move(Transform target)
    {
        throw new System.NotImplementedException();
    }
}
