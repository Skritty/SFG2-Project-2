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

    public CardUI card;
    public GameObject obj;

    public int ownerIndex = 0;
    public bool enabled = false;

    public Construction(ConstructionData _data, int _ownerIndex, CardUI defaultCard)
    {
        data = _data;
        durability = data.Durability;
        ownerIndex = _ownerIndex;

        if(defaultCard != null)
        {
            card = GameObject.Instantiate(defaultCard.gameObject).GetComponent<CardUI>();
            card.cardName.text = data.name;
            card.powerCost.text = "" + data.PowerConsumption;
            card.description.text = data.Description;
            card.image = data?.Image;
            card.info = this;
        }
    }

    public void Build(Tile tile)
    {
        obj = GameObject.Instantiate(data.BoardPiece, tile.transform.position, Quaternion.identity, tile.transform);
        tile.contains.Add(this);
    }
}
