using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : ICommand
{
    Construction self;

    public Toggle(Construction _self)
    {
        self = _self;
    }

    public void Execute()
    {
        self.Data.Toggle(self);
    }

    public void Undo()
    {

    }
}
