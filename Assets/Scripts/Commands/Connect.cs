using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect : ICommand
{
    Construction source;
    Construction target;
    ColorType color;

    public Connect(Construction _source, Construction _target, ColorType _color)
    {
        source = _source;
        target = _target;
        color = _color;
    }

    public void Execute()
    {
        WireConnection c = GameObject.Instantiate(GameManager.manager.UIManager.wire, source.obj.transform).GetComponent<WireConnection>();
        c.BuildWire(source, target, color);
        if (!target.color.HasFlag(color)) target.color = target.color | color;
    }

    public void Undo()
    {

    }
}
