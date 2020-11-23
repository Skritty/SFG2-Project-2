using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Obelisk", menuName = "Constructions/Board/Obelisk")]
public class Obelisk : ConstructionData
{
    public override void DoTurn(Construction target)
    {
        GameManager.CurrentPlayer.obelistPowerCurrent += 2 * target.inputConnections;
    }
}
