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
    public int interactionRange = 2;

    [Header("Turn Info")]
    public int cardsPlayed = 0;
    public int actionsDone = 0;
    public int extraCardsPlayed = 0;
    public int extraActionsDone = 0;

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
    public List<Construction> ownedOnBoard = new List<Construction>();

    [Header("Power")]
    [SerializeField] int coreMaxPower = 100;
    public int CoreMaxPower => coreMaxPower;
    public int coreCurrentPower;
    [SerializeField] int obeliskPowerToWin = 20;
    public int ObeliskPowerToWin => obeliskPowerToWin;
    public int obelistPowerCurrent;
    public Construction powerGridRoot;

    [Header("UI")]
    public CardUI defaultCard;

    private void Start()
    {
        coreCurrentPower = CoreMaxPower;
        obelistPowerCurrent = 0;
    }
}