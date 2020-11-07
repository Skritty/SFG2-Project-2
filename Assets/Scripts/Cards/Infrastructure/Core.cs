using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Core", menuName = "Constructions/Board/Core")]
public class Core : PowerTransferable
{
    public override void Connect(Construction self, Construction target, ColorType type)
    {
        base.Connect(self, target, type);
    }
}
