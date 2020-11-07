using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerTransferable : ConstructionData
{
    [Header("Power Transfer")]
    [SerializeField] int outputRate;
    public int OutputRate => outputRate;

    [SerializeField] int connectionRange = 2;
    public int ConnectionRange => connectionRange;

    [SerializeField] int maxConnections = 4;
    public int MaxConnections => maxConnections;

    public virtual void Connect(Construction self, Construction target, ColorType type)
    {

    }
}
