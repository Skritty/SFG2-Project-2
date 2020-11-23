using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : ICommand
{
    Construction self;
    public Move(Construction _self)
    {
        self = _self;
    }

    public void Execute()
    {
        // Move thing
    }

    public void Undo()
    {

    }
}
