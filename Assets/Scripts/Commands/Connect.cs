using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect : ICommand
{
    public static Construction source;
    Construction target;
    Tile tile;

    public Connect(Construction _self, Tile _tile)
    {
        target = _self;
        tile = _tile;
    }

    public void Execute()
    {
        if(source == null)
        {
            PowerTransferable d = (PowerTransferable)target.Data;
            if (d == null) return;
            source = target;
            foreach(Tile t in GameManager.manager.board.GetConstructionTilesInRadius(tile, d.ConnectionRange))
            {
                Construction c = t.contains.Find(x => x != null && x.Data.GetType() != typeof(Tracks));
            }
        }
        else
        {
            PowerTransferable d = (PowerTransferable)source.Data;
            if (d == null) return;
            WireConnection c = GameObject.Instantiate(GameManager.manager.UIManager.wire, source.obj.transform).GetComponent<WireConnection>();
            c.BuildWire(source, target, d.OutputRate);
            target.connections.Add(c);
            source.connections.Add(c);
            source = null;
        }
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }
}
