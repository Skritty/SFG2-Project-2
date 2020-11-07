using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [System.Serializable]
    private class InitialTile
    {
        public ConstructionData construction;
        public int playerIndex;
        public int x;
        public int y;

        public void Build(Tile tile)
        {
            Construction c;
            tile.contains.Add(c = new Construction(construction, playerIndex, null));
            c.obj = GameObject.Instantiate(construction.BoardPiece, tile.transform.position, Quaternion.identity, tile.transform);
            tile.contains.Add(c);
        }
    }

    [SerializeField] Tile tile;
    [SerializeField] InitialTile[] initialTiles;
    [SerializeField] Vector2 boardRealDimensions;
    [SerializeField] int boardWidth;
    [SerializeField] int boardHeight;

    public Tile[][] board;

    private void Start()
    {
        if (tile == null) return;
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        board = new Tile[boardWidth][];
        float distBetweenTilesX = boardRealDimensions.x / boardWidth;
        float distBetweenTilesY = boardRealDimensions.y / boardHeight;

        for(int i = 0; i < boardWidth; i++)
        {
            board[i] = new Tile[boardHeight];
            for(int j = 0; j < boardHeight; j++)
            {
                float h = (-distBetweenTilesX * boardWidth + 1) / 2 + distBetweenTilesX * i;
                float w = (-distBetweenTilesY * boardHeight + 1) / 2 + distBetweenTilesY * j;
                board[i][j] = Instantiate(tile.gameObject, transform.position + new Vector3(w, 0, h), Quaternion.identity, transform).GetComponent<Tile>();
                board[i][j].transform.localScale = new Vector3(distBetweenTilesX, 1, distBetweenTilesY);
                InitialTile initial = System.Array.Find(initialTiles, t => t.x == i && t.y == j);
                if (initial != null) initial.Build(board[i][j]);
            }
        }
    }

    public List<Tile> GetConstructionTilesInRadius(Tile tile, int radius)
    {
        List<Tile> tiles = new List<Tile>();
        for (int i = Mathf.Clamp(tile.x - radius, 0, boardWidth); i < Mathf.Clamp(tile.x + radius, 0, boardWidth); i++)
        {
            for (int j = Mathf.Clamp(tile.y - radius, 0, boardWidth); j < Mathf.Clamp(tile.y + radius, 0, boardWidth); j++)
            {
                if(board[i][j].contains.Find(x => x != null && x.Data.GetType() != typeof(Tracks)) != null)
                {
                    tiles.Add(tile);
                }
            }
        }
        return tiles;
    }
}
