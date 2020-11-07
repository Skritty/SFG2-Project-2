using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Barricade", menuName = "Constructions/Infrastructure/Barricade")]
public class Barricade : ConstructionData, IRailCompatable
{
    public void Move(Transform target)
    {
        throw new System.NotImplementedException();
    }
}
