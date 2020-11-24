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
    
    

    [Header("Menu References")]
    public Canvas playerCanvas;
    public GameObject mainMenu;
    public GameObject winScreen;
    public GameObject loseScreen;
    public Button playerEndTurnButton;
    public Transform playerEndTurnButtonPos1;
    public Transform playerEndTurnButtonPos2;
    public TextMeshProUGUI playerEndTurnButtonText;
    public TextMeshProUGUI boardPositionText;
    public TextMeshProUGUI Player1CorePower;
    public TextMeshProUGUI Player1ObeliskPower;
    public TextMeshProUGUI Player2CorePower;
    public TextMeshProUGUI Player2ObeliskPower;

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
    List<UICommandElement> selections = new List<UICommandElement>();

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
        GameInput.OnTileDeselect += ClearSelections;
        GameInput.WhileTileHovered += ShowHoverInfo;
        GameInput.OnTileHoverExit += HideHoverInfo;
    }

    public void UpdateUI()
    {
        Player1CorePower.text = "" + GameManager.manager.players[0].coreCurrentPower + "/" + GameManager.manager.players[0].CoreMaxPower;
        Player1ObeliskPower.text = "" + GameManager.manager.players[0].obelistPowerCurrent + "/" + GameManager.manager.players[0].ObeliskPowerToWin;
        Player2CorePower.text = "" + GameManager.manager.players[1].coreCurrentPower + "/" + GameManager.manager.players[1].CoreMaxPower;
        Player2ObeliskPower.text = "" + GameManager.manager.players[1].obelistPowerCurrent + "/" + GameManager.manager.players[1].ObeliskPowerToWin;

        DisplayDeck(player.machineDeck, machineDeckAnchor, true, settings.DeckCardDistBetween, 1f);
        DisplayDeck(player.infrastructureDeck, infrastructureDeckAnchor, true, settings.DeckCardDistBetween, 1f);
        DisplayHand(player.hand, handAnchor, false, settings.HandCardDistBetween, 1f);
        DisplayDeck(player.discard, discardAnchor, false, settings.DiscardCardDistBetween, 1f);

        DisplayDeck(otherPlayer.machineDeck, otherMachineDeckAnchor, true, settings.DeckCardDistBetween, settings.EnemyCardScale);
        DisplayDeck(otherPlayer.infrastructureDeck, otherInfrastructureDeckAnchor, true, settings.DeckCardDistBetween, settings.EnemyCardScale);
        DisplayHand(otherPlayer.hand, otherHandAnchor, true, settings.EnemyHandCardDistBetween, settings.EnemyCardScale);
        DisplayDeck(otherPlayer.discard, otherDiscardAnchor, false, settings.DiscardCardDistBetween, settings.EnemyCardScale);
    }

    public void FlipAndLerpCard()
    {

    }

    public void HideButton()
    {
        StartCoroutine(LerpButton(.5f, false));
    }

    public void ShowButton()
    {
        StartCoroutine(LerpButton(.5f, true));
    }

    private IEnumerator LerpButton(float time, bool reverse)
    {
        Vector2 dist = (playerEndTurnButtonPos1.position - playerEndTurnButtonPos2.position) * (reverse ? -1 : 1);
        for(int i = 0; i < 100; i++)
        {
            playerEndTurnButton.transform.position += (Vector3)dist / 100f;
            yield return new WaitForSecondsRealtime(time / 100f);
        }
    }

    private void DisplayDeck(Deck<Construction> deck, Transform anchor, bool flip, Vector2 dist, float scale)
    {
        for(int i = 0; i < deck.Count; i++)
        {
            //deck.GetCard(i).card.Transform(Vector3.zero, scale);
            Vector2 pos = (-dist * deck.Count + Vector2.right) / 2 + settings.DeckCardDistBetween * i;
            deck.GetCard(i).card.transform.parent = anchor;
            deck.GetCard(i).card.transform.localPosition = pos;
            deck.GetCard(i).card.Flip(flip);
            deck.GetCard(i).card.UpdateInitialTransform();
        }
    }

    private void DisplayHand(List<Construction> hand, Transform anchor, bool flip, Vector2 dist, float scale)
    {
        for (int i = hand.Count-1; i >= 0; i--)
        {
            //hand[i].card.Transform(Vector3.zero, scale);Debug.Log(scale);
            Vector2 pos = (-dist * hand.Count + Vector2.right) / 2 + dist * i;
            hand[i].card.transform.parent = anchor;
            hand[i].card.transform.SetAsLastSibling();
            hand[i].card.transform.localPosition = pos;
            hand[i].card.Flip(flip);
            hand[i].card.UpdateInitialTransform();
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
        tile?.RemoveMaterial(settings.TileHovered);
        tile?.RemoveMaterial(settings.TileSelected);
    }

    private void ShowHoverInfo(Tile tile)
    {
        if(boardPositionText != null)
        {
            boardPositionText.gameObject.SetActive(true);
            boardPositionText.transform.position = Input.mousePosition + new Vector3(0, 0);
            boardPositionText.text = "(" + tile.x + ',' + tile.y + ')';
        }
        
        if (!hoverInfo.gameObject.activeSelf && tile != null && tile.contains != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(tile.transform.position + settings.RelPosFromTile);
            hoverInfo.transform.position = screenPos;
            hoverInfo.cardName.text = tile.contains.Data.name;
            hoverInfo.powerUsage.text = ""+ tile.contains.Data.PowerConsumption;
            hoverInfo.powerIntake.text = ""+ tile.contains.powerIntake;
            hoverInfo.powerOutput.text = ""+ tile.contains.powerOutput;
            hoverInfo.gameObject.SetActive(true);
            foreach (Tile t in GameManager.manager.board.GetAllTilesInRadius(tile, tile.contains.Data.EffectRange)) t.SetMaterial(settings.HoverAoE);
        }
    }

    private void HideHoverInfo(Tile tile)
    {
        if (boardPositionText != null)
        {
            boardPositionText.gameObject.SetActive(false);
        }

        if (tile == null || tile.contains == null) return;
        foreach (Tile t in GameManager.manager.board.GetAllTilesInRadius(tile, tile.contains.Data.EffectRange)) t.RemoveMaterial(settings.HoverAoE);
        hoverInfo.gameObject.SetActive(false);
    }

    public void ShowConnectionPoints(Construction c, ColorType color)
    {
        foreach (Tile t in GameManager.manager.board.GetConstructionTilesInRadius(c.tile, c.Data.EffectRange))
        {
            if (t.contains == null || t.contains.inputConnections >= t.contains.Data.MaxInputConnections || (t.contains.ownerIndex != GameManager.manager.currentPlayer && t.contains.accessProtected)) continue;
            foreach (WireConnection wc in t.contains.connections) if (wc.target == c) continue;
            foreach (WireConnection wc in c.connections) if (wc.target == t.contains) continue;
            UICommandElement ui = null;
            switch (color)
            {
                case ColorType.White:
                    ui = GameObject.Instantiate(settings.ConnectionWhite, playerCanvas.transform).GetComponent<UICommandElement>();
                    break;
                case ColorType.Red:
                    ui = GameObject.Instantiate(settings.ConnectionRed, playerCanvas.transform).GetComponent<UICommandElement>();
                    break;
                case ColorType.Green:
                    ui = GameObject.Instantiate(settings.ConnectionGreen, playerCanvas.transform).GetComponent<UICommandElement>();
                    break;
                case ColorType.Blue:
                    ui = GameObject.Instantiate(settings.ConnectionBlue, playerCanvas.transform).GetComponent<UICommandElement>();
                    break;
            }
            if (ui == null) continue;
            ui.transform.position = Camera.main.WorldToScreenPoint(t.transform.position + settings.RelPosFromTile);
            ui.tile = t;
            selections.Add(ui);
        }
    }

    public void ShowTargetPoints(Construction c)
    {
        foreach (Tile t in GameManager.manager.board.GetConstructionTilesInRadius(c.tile, c.Data.EffectRange))
        {
            if (!t.contains.targetable) return;
            UICommandElement ui = GameObject.Instantiate(settings.SelectionTarget, playerCanvas.transform).GetComponent<UICommandElement>();
            ui.transform.position = Camera.main.WorldToScreenPoint(t.transform.position + settings.RelPosFromTile);
            ui.tile = t;
            selections.Add(ui);
        }
    }

    public void ShowDirections(Tile tile)
    {
        foreach (Tile t in GameManager.manager.board.GetAllTilesInRadius(tile, 1))
        {
            if (!t.contains.targetable) return;
            UICommandElement ui = GameObject.Instantiate(settings.SelectionTarget, playerCanvas.transform).GetComponent<UICommandElement>();
            ui.transform.position = Camera.main.WorldToScreenPoint(t.transform.position + settings.RelPosFromTile);
            ui.tile = t;
            selections.Add(ui);
        }
    }

    public void ClearSelections(Tile t)
    {
        foreach (UICommandElement e in selections) GameObject.Destroy(e.gameObject);
        selections.Clear();
    }
}
