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

    [SerializeField] int effectRange = 0;
    public int EffectRange => effectRange;

    [SerializeField] int cardRating = 0;
    public int CardRating => cardRating;

    [SerializeField] string description;
    public string Description => description;

    [Header("Board and UI")]
    [SerializeField] Sprite image;
    public Sprite Image => image;

    [SerializeField] GameObject boardPiece;
    public GameObject BoardPiece => boardPiece;

    [SerializeField] SelectUIElements selectionUI;
    public SelectUIElements SelectionUI => selectionUI;

    [SerializeField] CardHoverInfo hoverUI;
    public CardHoverInfo HoverUI => hoverUI;

    [Header("Power Transfer")]
    [SerializeField] ColorType initialColor = ColorType.White;
    public ColorType InitialColor => initialColor;

    [SerializeField] int outputRate;
    public int OutputRate => outputRate;

    [SerializeField] int maxOutputConnections = 4;
    public int MaxOutputConnections => maxOutputConnections;

    [SerializeField] int maxInputConnections = 1;
    public int MaxInputConnections => maxInputConnections;

    public virtual void DoTurn(Construction target)
    {

    }

    public virtual void Activate(Construction target)
    {

    }

    public virtual void Toggle(Construction target)
    {

    }

    public void ConsumePower(Construction target)
    {
        
    }

    public virtual void Connect(Construction self, Construction target, ColorType type)
    {

    }

    public virtual int CalculateChoiceRating(Tile tile, Construction construction)
    {
        List<Construction> nearbyAllyConstructions = new List<Construction>();
        List<Construction> nearbyEnemyConstructions = new List<Construction>();
        int distanceToCore = Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[1].tile.x - tile.x) + Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[1].tile.y - tile.y);
        int distanceToObelisk = Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[0].tile.x - tile.x) + Mathf.Abs(GameManager.CurrentPlayer.ownedOnBoard[0].tile.y - tile.y);

        if (tile.contains != null) return -100;
        foreach (Tile t in GameManager.manager.board.GetConstructionTilesInRadius(tile, construction.Data.EffectRange))
        {
            if (t.contains.ownerIndex == GameManager.manager.currentPlayer)
            {
                nearbyAllyConstructions.Add(t.contains);
            }
            else
            {
                nearbyEnemyConstructions.Add(t.contains);
            }
        }

        return distanceToCore - distanceToObelisk - nearbyAllyConstructions.Count + nearbyEnemyConstructions.Count;
    }
}
