using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Info")]
    public int playerIndex;
    public int cardsPlayedPerTurn = 1;
    public int actionsPerTurn = 1;

    [Header("Decks")]
    public int startingMachineCards = 2;
    public int startingInfrastructureCards = 2;

    // Initial deck data
    public ConstructionData[] machineDeckInitial;
    public ConstructionData[] infrastructureDeckInitial;
    public ConstructionData[] handInitial;
    public ConstructionData[] discardInitial;

    // In-game deck data
    public Deck<Construction> machineDeck = new Deck<Construction>();
    public Deck<Construction> infrastructureDeck = new Deck<Construction>();
    public List<Construction> hand = new List<Construction>();
    public Deck<Construction> discard = new Deck<Construction>();

    [Header("UI")]
    public CardUI defaultCard;
}