using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConstructionData : ScriptableObject
{
    [Header("Basic Construction")]
    [SerializeField] int durability = 1;
    public int Durability => durability;

    [SerializeField] int cardPlayCost = 1;
    public int CardPlayCost => cardPlayCost;

    [SerializeField] int powerConsumption = 0;
    public int PowerConsumption => powerConsumption;

    [SerializeField] string description;
    public string Description => description;

    [SerializeField] Sprite image;
    public Sprite Image => image;

    [SerializeField] GameObject boardPiece;
    public GameObject BoardPiece => boardPiece;

    [SerializeField] SelectUIElements selectionUI;
    public SelectUIElements SelectionUI => selectionUI;

    [SerializeField] CardHoverInfo hoverUI;
    public CardHoverInfo HoverUI => hoverUI;

    public virtual void DoTurn(Construction target)
    {

    }

    public virtual void Activate(Construction target)
    {

    }

    public void Damage()
    {

    }

    public void ConsumePower(Construction target)
    {
        
    }
}
