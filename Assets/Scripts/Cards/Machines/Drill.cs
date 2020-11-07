using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drill", menuName = "Constructions/Machines/Drill")]
public class Drill : ConstructionData, IRailCompatable
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
