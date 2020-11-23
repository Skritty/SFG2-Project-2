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
}
