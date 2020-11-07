using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Laser", menuName = "Constructions/Machines/Laser")]
public class Laser : ConstructionData, IRailCompatable
{
    public override void Activate(Construction target)
    {
        target.enabled = !target.enabled;
        Debug.Log(target.enabled);
    }

    public override void DoTurn(Construction target)
    {
        base.DoTurn(target);
    }

    public void Move(Transform target)
    {
        throw new System.NotImplementedException();
    }
}

