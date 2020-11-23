using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : ICommand
{
    Construction self;
    Tile target;
    public Rotate(Construction _self, Tile _target)
    {
        self = _self;
        target = _target;
    }

    public void Execute()
    {
        // Rotate thing
    }

    public void Undo()
    {

    }
}
