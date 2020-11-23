using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WireConnection : MonoBehaviour
{
    public Construction source { get; private set; }
    public Construction target { get; private set; }

    [Header("Wire")]
    public ColorType type;
    Vector3 offset = new Vector3(0,.4f,0);

    public void BuildWire(Construction _source, Construction _target, ColorType color)
    {
        LineRenderer line = GetComponent<LineRenderer>();
        source = _source;
        target = _target;
        type = color;
        line.SetPosition(0, source.obj.transform.position + offset);
        line.SetPosition(1, target.obj.transform.position + offset);
        switch (type)
        {
            case ColorType.White:
                line.sharedMaterial = GameManager.manager.UIManager.settings.WireWhite;
                break;
            case ColorType.Red:
                line.sharedMaterial = GameManager.manager.UIManager.settings.WireRed;
                break;
            case ColorType.Green:
                line.sharedMaterial = GameManager.manager.UIManager.settings.WireGreen;
                break;
            case ColorType.Blue:
                line.sharedMaterial = GameManager.manager.UIManager.settings.WireBlue;
                break;
        }
        source.connections.Add(this);
        target.inputConnections++;
    }
}
