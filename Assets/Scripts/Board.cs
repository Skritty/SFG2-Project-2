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
            tile.contains = new Construction(construction, playerIndex, null);
            tile.contains.tile = tile;
            tile.contains.obj = GameObject.Instantiate(construction.BoardPiece, tile.transform.position, Quaternion.identity, tile.transform);
            GameManager.manager.players[playerIndex].ownedOnBoard.Add(tile.contains);
            if (construction.name == "Core") GameManager.manager.players[playerIndex].powerGridRoot = tile.contains;Debug.Log(tile.contains.Data.name);
        }
    }

    [SerializeField] Tile tile;
    [SerializeField] InitialTile[] initialTiles;
    [SerializeField] Vector2 boardRealDimensions;
    [SerializeField] int boardWidth;
    [SerializeField] int boardHeight;

    public Vector2 distBetween;

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
        distBetween = new Vector2(distBetweenTilesX, distBetweenTilesY);

        for(int i = 0; i < boardWidth; i++)
        {
            board[i] = new Tile[boardHeight];
            for(int j = 0; j < boardHeight; j++)
            {
                float h = (-distBetweenTilesX * boardWidth + 1) / 2 + distBetweenTilesX * i;
                float w = (-distBetweenTilesY * boardHeight + 1) / 2 + distBetweenTilesY * j;
                board[i][j] = Instantiate(tile.gameObject, transform.position + new Vector3(w, 0, h), Quaternion.identity, transform).GetComponent<Tile>();
                board[i][j].transform.localScale = new Vector3(distBetweenTilesX, 1, distBetweenTilesY);
                board[i][j].x = i;
                board[i][j].y = j;
                InitialTile initial = System.Array.Find(initialTiles, t => t.x == i && t.y == j);
                if (initial != null) initial.Build(board[i][j]);
            }
        }
    }

    public void LowerBuildsRemaining(int amt)
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                board[i][j].buildsRemaining -= amt;
                board[i][j].buildsRemaining = Mathf.Clamp(board[i][j].buildsRemaining, 0, 100000);
            }
        }
    }

    public bool HasBuildSpot()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if(board[i][j].buildsRemaining > 0) return true;
            }
        }
        return false;
    }

    public List<Tile> GetConstructionTilesInRadius(Tile tile, int radius)
    {
        List<Tile> tiles = new List<Tile>();
        for (int i = Mathf.Clamp(tile.x - radius, 0, boardWidth - 1); i <= Mathf.Clamp(tile.x + radius, 0, boardWidth - 1); i++)
        {
            for (int j = Mathf.Clamp(tile.y - radius, 0, boardWidth - 1); j <= Mathf.Clamp(tile.y + radius, 0, boardWidth - 1); j++)
            {
                if (i == tile.x && j == tile.y || (Mathf.Abs(i - tile.x) + Mathf.Abs(j - tile.y) > radius + 1)) continue;
                if(board[i][j].contains != null)
                {
                    tiles.Add(board[i][j]);
                }
            }
        }
        return tiles;
    }

    public List<Tile> GetAllTilesInRadius(Tile tile, int radius)
    {
        List<Tile> tiles = new List<Tile>();
        for (int i = Mathf.Clamp(tile.x - radius, 0, boardWidth - 1); i <= Mathf.Clamp(tile.x + radius, 0, boardWidth - 1); i++)
        {
            for (int j = Mathf.Clamp(tile.y - radius, 0, boardWidth - 1); j <= Mathf.Clamp(tile.y + radius, 0, boardWidth - 1); j++)
            {
                if (i == tile.x && j == tile.y || (Mathf.Abs(i - tile.x) + Mathf.Abs(j - tile.y) > radius + 1)) continue;
                tiles.Add(board[i][j]);
            }
        }
        return tiles;
    }

    public bool TileInPlayerRange(int playerIndex, Tile tile, int radius)
    {
        foreach(Construction c in GameManager.CurrentPlayer.ownedOnBoard)
        {
            Tile t = c.tile;
            for (int i = Mathf.Clamp(t.x - radius, 0, boardWidth - 1); i <= Mathf.Clamp(t.x + radius, 0, boardWidth - 1); i++)
            {
                for (int j = Mathf.Clamp(t.y - radius, 0, boardWidth - 1); j <= Mathf.Clamp(t.y + radius, 0, boardWidth - 1); j++)
                {
                    if ((Mathf.Abs(i - t.x) + Mathf.Abs(j - t.y) > radius + 1)) continue;
                    if (board[i][j] == tile) return true;
                }
            }
        }
        return false;
    }

    public bool TileInConstructionRange(Construction c, Tile tile)
    {
        Tile t = c.tile;
        int radius = c.Data.EffectRange;
        for (int i = Mathf.Clamp(t.x - radius, 0, boardWidth - 1); i <= Mathf.Clamp(t.x + radius, 0, boardWidth - 1); i++)
        {
            for (int j = Mathf.Clamp(t.y - radius, 0, boardWidth - 1); j <= Mathf.Clamp(t.y + radius, 0, boardWidth - 1); j++)
            {
                if ((Mathf.Abs(i - t.x) + Mathf.Abs(j - t.y) > radius + 1)) continue;
                if (board[i][j] == tile) return true;
            }
        }
        return false;
    }

    public List<Tile> GetTilesInPlayerRange(int playerIndex, int radius)
    {
        List<Tile> tiles = new List<Tile>();
        foreach (Construction c in GameManager.CurrentPlayer.ownedOnBoard)
        {
            Tile tile = c.tile;
            for (int i = Mathf.Clamp(tile.x - radius, 0, boardWidth - 1); i <= Mathf.Clamp(tile.x + radius, 0, boardWidth - 1); i++)
            {
                for (int j = Mathf.Clamp(tile.y - radius, 0, boardWidth - 1); j <= Mathf.Clamp(tile.y + radius, 0, boardWidth - 1); j++)
                {
                    if (i == tile.x && j == tile.y || (Mathf.Abs(i - tile.x) + Mathf.Abs(j - tile.y) > radius + 1) || board[i][j] == tile) continue;
                    tiles.Add(board[i][j]);
                }
            }
        }
        return tiles;
    }

    public List<Tile> GetPoweredTilesInPlayerRange(int playerIndex)
    {
        List<Tile> tiles = new List<Tile>();
        foreach (Construction c in GameManager.CurrentPlayer.ownedOnBoard)
        {
            if (!(c.Data is IPowerTransferable) || c.connections.Count >= c.Data.MaxOutputConnections - 1 || c.tile.buildsRemaining == 0) continue;
            Tile tile = c.tile;
            int radius = c.Data.EffectRange;
            for (int i = Mathf.Clamp(tile.x - radius, 0, boardWidth - 1); i <= Mathf.Clamp(tile.x + radius, 0, boardWidth - 1); i++)
            {
                for (int j = Mathf.Clamp(tile.y - radius, 0, boardWidth - 1); j <= Mathf.Clamp(tile.y + radius, 0, boardWidth - 1); j++)
                {
                    if (i == tile.x && j == tile.y || (Mathf.Abs(i - tile.x) + Mathf.Abs(j - tile.y) > radius + 1) || board[i][j] == tile) continue;
                    tiles.Add(board[i][j]);
                }
            }
        }
        return tiles;
    }

    public void ResetTileBuffs()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                board[i][j].ResetBuffs();
            }
        }
    }
}
