using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is the "Card"
[System.Serializable]
public class Construction
{
    [SerializeField] ConstructionData data;
    public ConstructionData Data { get { return data; } }

    [SerializeField] int durability = 1;
    public int Durability { get { return durability; } set { durability = value; } }

    public List<WireConnection> connections = new List<WireConnection>();
    public int inputConnections = 0;
    public int powerIntake = 0;
    public int powerOutput = 0;

    public CardUI card;
    public GameObject obj;
    public Tile tile;

    public int ownerIndex = 0;
    public bool enabled = false;
    public bool powered = false;
    public bool accessProtected = false;
    public bool targetable = true;
    public ColorType color = ColorType.White;

    public Construction(ConstructionData _data, int _ownerIndex, CardUI defaultCard)
    {
        data = _data;
        durability = data.Durability;
        ownerIndex = _ownerIndex;
        color = data.InitialColor;

        if(defaultCard != null)
        {
            card = GameObject.Instantiate(defaultCard.gameObject).GetComponent<CardUI>();
            card.cardName.text = data.name;
            card.powerCost.text = "" + data.PowerConsumption;
            card.cardCost.text = "" + data.CardPlayCost;
            card.description.text = data.Description;
            card.image = data?.Image;
            card.info = this;
        }
    }

    public void Build(Tile tile)
    {
        obj = GameObject.Instantiate(data.BoardPiece, tile.transform.position, Quaternion.identity, tile.transform);
        if (card.info.Data.GetType() == typeof(Tracks)) tile.rail = this;
        else tile.contains = this;
    }

    public void TransferPower(int power)
    {
        BuffReset();
        powerIntake = power;
        powerOutput = 0;
        power -= Data.PowerConsumption;
        if (power < 0) return;
        powered = true;
        GameManager.CurrentPlayer.coreCurrentPower -= Data.PowerConsumption;
        durability--;
        if (durability == 0) Destroy();
        if (power == 0) return;
        powerOutput = power;
        foreach (WireConnection c in connections)
        {
            c.target.TransferPower(Mathf.Clamp(power / connections.Count, 0, data.OutputRate));
        }
    }

    public void BuffReset()
    {
        powered = false;
        accessProtected = false;
    }

    public void Destroy()
    {

    }
}
