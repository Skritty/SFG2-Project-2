﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recycler", menuName = "Constructions/Machines/Recycler")]
public class Recycler : ConstructionData, IRailCompatable
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