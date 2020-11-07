using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GameInput : MonoBehaviour
{
    public static Action<Tile> OnTileHoverEnter = delegate { };
    public static Action<Tile> WhileTileHovered = delegate { };
    public static Action<Tile> OnTileHoverExit = delegate { };

    public static Action<Tile> OnTileSelect = delegate { };
    public static Action<Tile> WhileTileSelected = delegate { };
    public static Action<Tile> OnTileDeselect = delegate { };

    public static Action<CardUI> OnCardHoverEnter = delegate { };
    public static Action<CardUI> WhileCardHovered = delegate { };
    public static Action<CardUI> OnCardHoverExit = delegate { };

    public static Action<CardUI, Tile> OnCardPickup = delegate { };
    public static Action<CardUI, Tile> WhileCardHeld = delegate { };
    public static Action<CardUI, Tile> OnCardDropped = delegate { };

    public static Action<SelectUIElements, Tile> OnClickCommand = delegate { };

    [Header("UI references")]
    [SerializeField] GraphicRaycaster canvas;
    GraphicRaycaster raycaster;
    PointerEventData pointerEvent;
    EventSystem eventSystem;

    List<RaycastResult> results;

    Tile selectedTile;
    Tile hoveredTile;
    CardUI clickedCard;
    CardUI hoveredCard;

    private void Start()
    {
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = canvas.GetComponent<EventSystem>();
    }

    private void Update()
    {
        pointerEvent = new PointerEventData(eventSystem);
        pointerEvent.position = Input.mousePosition;
        results = new List<RaycastResult>();
        raycaster.Raycast(pointerEvent, results);

        SelectInteraction();
        TileInteraction();
        CardInteraction();
        Tick();
    }

    private void Tick()
    {
        if (hoveredCard != null) WhileCardHovered.Invoke(hoveredCard);
        if (clickedCard != null) WhileCardHeld.Invoke(clickedCard, hoveredTile);
        if (hoveredTile != null) WhileTileHovered.Invoke(hoveredTile);
        if (selectedTile != null) WhileTileSelected.Invoke(selectedTile);
    }

    private void CardInteraction()
    {
        CardUI card = null;
        if (results.Count > 0)
            card = results[0].gameObject.GetComponentInParent<CardUI>();

        // When moving the mouse onto or away from a card
        if(card != hoveredCard && (clickedCard == null || card != clickedCard))
        {
            if(hoveredCard != null) OnCardHoverExit.Invoke(hoveredCard);
            hoveredCard = card;
            if (hoveredCard != null) OnCardHoverEnter.Invoke(hoveredCard);
        }

        // Click on card
        if (Input.GetMouseButtonDown(0) && card != null)
        {
            if(hoveredTile != null)
            {
                OnCardHoverExit.Invoke(hoveredCard);
                hoveredCard = null;
            }
            
            clickedCard = card;
            OnCardPickup.Invoke(clickedCard, hoveredTile);
        }

        // Click let go
        if (Input.GetMouseButtonUp(0) && clickedCard != null)
        {
            OnCardDropped.Invoke(clickedCard, hoveredTile);
            clickedCard = null;
        }
    }

    private void TileInteraction()
    {
        Tile tile = null;
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("UI");
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, mask))
        {
            tile = hit.transform.GetComponent<Tile>();
        }

        // When hovering the mouse onto or away from a tile
        if(tile != hoveredTile)
        {
            if (hoveredTile != null) OnTileHoverExit.Invoke(hoveredTile);
            if(selectedTile == null || tile != selectedTile)
            {
                hoveredTile = tile;
                if (hoveredTile != null) OnTileHoverEnter.Invoke(hoveredTile);
            }
        }
        
        // Click on tile (a tile cannot be hovered and selected at the same time)
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedTile != null && selectedTile == tile)
            {
                OnTileDeselect.Invoke(selectedTile);
                hoveredTile = selectedTile;
                OnTileHoverEnter.Invoke(hoveredTile);
                selectedTile = null;
            }
            else
            {
                if (selectedTile != null) OnTileDeselect.Invoke(selectedTile);
                selectedTile = tile;
                if (selectedTile != null)
                {
                    if (hoveredTile != null) OnTileHoverExit.Invoke(hoveredTile);
                    hoveredTile = null;
                    OnTileSelect.Invoke(selectedTile);
                }
            }
        }
    }

    private void SelectInteraction()
    {
        if(Input.GetMouseButtonDown(0))
            foreach (RaycastResult result in results)
            {
                if (selectedTile && result.gameObject.GetComponent<UICommandElement>())
                {
                    Debug.Log(selectedTile.contains[0]);
                    OnClickCommand.Invoke(result.gameObject.GetComponent<UICommandElement>().element, selectedTile);
                    break;
                }
            }
    }
}
