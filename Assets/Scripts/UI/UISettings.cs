using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UISettings", menuName = "UI Settings")]
public class UISettings : ScriptableObject
{
    [Header("Field UI")]
    [SerializeField] GameObject hoverInfo;
    public GameObject HoverInfo => hoverInfo;

    [SerializeField] GameObject selectInfo;
    public GameObject SelectInfo => selectInfo;

    [SerializeField] Vector3 relPosFromTile;
    public Vector3 RelPosFromTile => relPosFromTile;

    [Header("Deck")]
    [SerializeField] Vector2 deckCardDistBetween;
    public Vector2 DeckCardDistBetween => deckCardDistBetween;

    [SerializeField] Vector2 handCardDistBetween;
    public Vector2 HandCardDistBetween => handCardDistBetween;

    [SerializeField] Vector2 discardCardDistBetween;
    public Vector2 DiscardCardDistBetween => discardCardDistBetween;


    [Header("Tile")]
    [SerializeField] Material tileSelected;
    public Material TileSelected => tileSelected;

    [SerializeField] Material tileHovered;
    public Material TileHovered => tileHovered;


    [Header("Card")]
    [SerializeField] float cardZoomScale = 2f;
    public float CardZoomScale => cardZoomScale;

    [SerializeField] Vector2 cardZoomMove = new Vector2(0, 30);
    public Vector2 CardZoomMove => cardZoomMove;

    [SerializeField] float cardMoveScale = 0.4f;
    public float CardMoveScale => cardMoveScale;
    
}