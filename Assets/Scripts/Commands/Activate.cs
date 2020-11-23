using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate : ICommand
{
    Construction self;

    public Activate(Construction _self)
    {
        self = _self;
    }

    public void Execute()
    {
        self.Data.Activate(self);
    }

    public void Undo()
    {

    }
}
