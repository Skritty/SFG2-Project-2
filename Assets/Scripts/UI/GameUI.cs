using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Core References")]
    public UISettings settings;
    [Space]
    [SerializeField] Player player;
    [SerializeField] Player otherPlayer;
    public GameObject wire; // Add to settings
    public GameObject connectionPoint;

    [Header("Menu References")]
    public Canvas playerCanvas;
    public GameObject mainMenu;
    public GameObject winScreen;
    public GameObject loseScreen;
    public Button playerEndTurnButton;
    public TextMeshProUGUI playerEndTurnButtonText;

    [Header("Card Anchors")]
    [SerializeField] Transform machineDeckAnchor;
    [SerializeField] Transform infrastructureDeckAnchor;
    [SerializeField] Transform handAnchor;
    [SerializeField] Transform discardAnchor;

    [SerializeField] Transform otherMachineDeckAnchor;
    [SerializeField] Transform otherInfrastructureDeckAnchor;
    [SerializeField] Transform otherHandAnchor;
    [SerializeField] Transform otherDiscardAnchor;

    int zoomCardPrevIndex;
    CardHoverInfo hoverInfo;
    [HideInInspector] public CardSelectInfo selectInfo;

    private void Start()
    {
        hoverInfo = Instantiate(settings.HoverInfo, playerCanvas.transform).GetComponent<CardHoverInfo>();
        hoverInfo.gameObject.SetActive(false);

        selectInfo = Instantiate(settings.SelectInfo, playerCanvas.transform).GetComponent<CardSelectInfo>();
        selectInfo.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameInput.OnCardHoverEnter += ZoomCard;
        GameInput.OnCardHoverExit += UnzoomCard;
        GameInput.OnTileHoverEnter += TileHoverFeedback;
        GameInput.OnTileHoverExit += TileFeedbackReset;
        GameInput.OnTileSelect += TileSelectedFeedback;
        GameInput.OnTileDeselect += TileFeedbackReset;
        GameInput.WhileTileHovered += ShowHoverInfo;
        GameInput.OnTileHoverExit += HideHoverInfo;
    }

    public void UpdateUI()
    {
        DisplayDeck(player.machineDeck, machineDeckAnchor);
        DisplayDeck(player.infrastructureDeck, infrastructureDeckAnchor);
        DisplayHand(player.hand, handAnchor);
        DisplayDiscard(player.discard, discardAnchor);

        /*DisplayDeck(otherPlayer.machineDeck, otherMachineDeckAnchor);
        DisplayDeck(otherPlayer.infrastructureDeck, otherInfrastructureDeckAnchor);
        DisplayHand(otherPlayer.hand, otherHandAnchor);
        DisplayDiscard(otherPlayer.discard, otherDiscardAnchor);*/
    }

    private void DisplayDeck(Deck<Construction> deck, Transform anchor)
    {
        for(int i = 0; i < deck.Count; i++)
        {
            Vector2 pos = (-settings.DeckCardDistBetween * deck.Count + Vector2.right) / 2 + settings.DeckCardDistBetween * i;
            deck.GetCard(i).card.transform.parent = anchor;
            deck.GetCard(i).card.transform.localPosition = pos;
            deck.GetCard(i).card.Flip(true);
            deck.GetCard(i).card.UpdateInitialTransform();
        }
    }

    private void DisplayHand(List<Construction> hand, Transform anchor)
    {
        for (int i = hand.Count-1; i >= 0; i--)
        {
            Vector2 pos = (-settings.HandCardDistBetween * hand.Count + Vector2.right) / 2 + settings.HandCardDistBetween * i;
            hand[i].card.transform.parent = anchor;
            hand[i].card.transform.SetAsLastSibling();
            hand[i].card.transform.localPosition = pos;
            hand[i].card.Flip(false);
            hand[i].card.UpdateInitialTransform();
        }
    }

    private void DisplayDiscard(Deck<Construction> discard, Transform anchor)
    {
        for (int i = 0; i < discard.Count; i++)
        {
            Vector2 pos = (-settings.DiscardCardDistBetween * discard.Count + Vector2.right) / 2 + settings.DiscardCardDistBetween * i;
            discard.GetCard(i).card.transform.parent = anchor;
            discard.GetCard(i).card.transform.localPosition = pos;
            discard.GetCard(i).card.Flip(false);
            discard.GetCard(i).card.UpdateInitialTransform();
        }
    }

    private void ZoomCard(CardUI card)
    {
        zoomCardPrevIndex = card.transform.GetSiblingIndex();
        card.transform.SetAsLastSibling();
        card.Transform(settings.CardZoomMove, settings.CardZoomScale, true);
    }

    private void UnzoomCard(CardUI card)
    {
        card.transform.SetSiblingIndex(zoomCardPrevIndex);
        card.ResetTransform();
    }

    private void TileHoverFeedback(Tile tile)
    {
        tile.SetMaterial(settings.TileHovered);
    }

    private void TileSelectedFeedback(Tile tile)
    {
        tile.SetMaterial(settings.TileSelected);
    }

    private void TileFeedbackReset(Tile tile)
    {
        tile?.ResetMaterial();
    }

    private void ShowHoverInfo(Tile tile)
    {
        if (!hoverInfo.gameObject.activeSelf && tile != null && tile.contains.Count > 0)
        {
            Construction c = tile.contains.Find(x => x != null && x.Data.GetType() != typeof(Tracks));
            if (c == null) return;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(tile.transform.position + settings.RelPosFromTile);
            hoverInfo.transform.position = screenPos;
            hoverInfo.cardName.text = c.Data.name;
            hoverInfo.powerUsage.text = ""+c.Data.PowerConsumption;
            hoverInfo.gameObject.SetActive(true);
        }
    }

    private void HideHoverInfo(Tile tile)
    {
        hoverInfo.gameObject.SetActive(false);
    }

    public void ShowConnectionPoint(Construction c, Tile t)
    {

    }
}
