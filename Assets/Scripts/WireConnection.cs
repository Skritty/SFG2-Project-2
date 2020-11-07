using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WireConnection : MonoBehaviour
{
    Vector3 offset = new Vector3(0,.4f,0);
    Construction source;
    Construction target;
    int maxTransfer;
    LineRenderer line;

    public void BuildWire(Construction _source, Construction _target, int transferRate)
    {
        line = GetComponent<LineRenderer>();
        source = _source;
        target = _target;
        maxTransfer = transferRate;
        line.SetPosition(0, source.obj.transform.position + offset);
        line.SetPosition(1, target.obj.transform.position + offset);
    }
}
