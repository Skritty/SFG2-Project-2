using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : ICommand
{
    Construction construction;
    Tile tile;

    public Toggle(Construction _construction, Tile _tile)
    {
        construction = _construction;
        tile = _tile;
    }

    public void Execute()
    {
        construction.Data.Activate(construction);
    }

    public void Undo()
    {

    }
}
