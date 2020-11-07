using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Factory", menuName = "Constructions/Machines/Factory")]
public class Factory : ConstructionData, IRailCompatable
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

