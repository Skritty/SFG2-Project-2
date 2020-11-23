using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetActivate : ICommand
{
    Construction source;
    Construction target;

    public TargetActivate(Construction _source, Construction _target)
    {
        source = _source;
        target = _target;
    }

    public void Execute()
    {
        source.Data.Activate(target);
    }

    public void Undo()
    {

    }
}