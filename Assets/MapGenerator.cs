using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileGridGenerator : MonoBehaviour
{
    public GameObject tilePrefab; 
    public int width = 6;
    public int height = 7;
    public float tileSpacing = 1.0f; 

    public int RandomTiles = 3;
    public Material roomMaterial;    

    private Tile[,] grid;
    private List<List<Tile>> rooms;

    void Start()
    {
        GenerateTileGrid();
        DetermineRooms();
        GrowRoomsAlternately(); 
    }

    void GenerateTileGrid()
    {
        grid = new Tile[width, height];

        //make a 2d loop... 
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //flat grid... 
                Vector3 position = new Vector3(x * tileSpacing, 0, y * tileSpacing);
                GameObject tileObject = Instantiate(tilePrefab, position, Quaternion.identity);
                tileObject.transform.parent = transform;  //put em in the generator... 

                Tile tile = tileObject.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.position = new Vector2Int(x, y);
                    grid[x, y] = tile;
                }
            }
        }

        //assign neighbors... 
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile left = (x > 0) ? grid[x - 1, y] : null;
                Tile right = (x < width - 1) ? grid[x + 1, y] : null;
                Tile up = (y < height - 1) ? grid[x, y + 1] : null;
                Tile down = (y > 0) ? grid[x, y - 1] : null;

                Tile topLeft = (x > 0 && y < height - 1) ? grid[x - 1, y + 1] : null;
                Tile topRight = (x < width - 1 && y < height - 1) ? grid[x + 1, y + 1] : null;
                Tile bottomLeft = (x > 0 && y > 0) ? grid[x - 1, y - 1] : null;
                Tile bottomRight = (x < width - 1 && y > 0) ? grid[x + 1, y - 1] : null;

                Tile currentTile = grid[x, y];
                if (currentTile != null)
                {
                    currentTile.SetNeighbors(left, right, up, down, topLeft, topRight, bottomLeft, bottomRight);
                }
            }
        }
    }

    public void DetermineRooms()
    {
        List<Tile> allTiles = new List<Tile>();
        foreach (Tile tile in grid)
        {
            if (tile != null)
            {
                allTiles.Add(tile);
            }
        }

        List<Tile> selectedTiles = new List<Tile>();
        HashSet<Tile> chosenTiles = new HashSet<Tile>();

        while (selectedTiles.Count < RandomTiles)
        {
            //pick a tile, any tile... 
            Tile randomTile = allTiles[Random.Range(0, allTiles.Count)];

            //check that it isn't a neighbor.... 
            bool isValid = true;
            foreach (Tile chosenTile in chosenTiles)
            {
                if (IsNeighbor(randomTile, chosenTile))
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                selectedTiles.Add(randomTile);
                chosenTiles.Add(randomTile);
            }
        }

        //gonna get rid of this later, it's just for debug purposes... 
        foreach (Tile tile in selectedTiles)
        {
            Renderer tileRenderer = tile.GetComponent<Renderer>();
            if (tileRenderer != null)
            {
                Material newMaterial = new Material(Shader.Find("Standard")); 
                newMaterial.color = Random.ColorHSV(); 
                tileRenderer.material = newMaterial;    
            }
        }
    }

    private bool IsNeighbor(Tile tile1, Tile tile2)
    {
        return tile1.neighborLeft == tile2 ||
               tile1.neighborRight == tile2 ||
               tile1.neighborUp == tile2 ||
               tile1.neighborDown == tile2 ||
               tile1.topLeft == tile2 ||
               tile1.topRight == tile2 ||
               tile1.bottomLeft == tile2 ||
               tile1.bottomRight == tile2;
    }


    private void GrowRoomsAlternately()
    {
        rooms = new List<List<Tile>>();
        List<Color> roomColors = new List<Color>();

        //init rooms with colored tiles.... 
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = grid[x, y];
                if (tile != null && tile.GetComponent<Renderer>().material.color != Color.white)
                {
                    List<Tile> room = new List<Tile> { tile };
                    rooms.Add(room);
                    roomColors.Add(tile.GetComponent<Renderer>().material.color);
                }
            }
        }

        //make sure we've got the right number of rooms based off the unique number of tiles selected... 
        while (rooms.Count < RandomTiles)
        {
            Tile randomTile = GetRandomUnassignedTile();
            if (randomTile != null)
            {
                List<Tile> newRoom = new List<Tile> { randomTile };
                rooms.Add(newRoom);
                roomColors.Add(Random.ColorHSV());
                randomTile.GetComponent<Renderer>().material.color = roomColors[roomColors.Count - 1];
            }
            else
            {
                break; //all done... 
            }
        }

        //lock bool... 
        bool growthOccurred;
        do
        {
            growthOccurred = false;
            for (int i = 0; i < rooms.Count; i++)
            {
                if (GrowRoomByOneTile(rooms[i], roomColors[i]))
                {
                    growthOccurred = true;
                }
            }
        } while (growthOccurred);

        //name the rooms... 
        for (int i = 0; i < rooms.Count; i++)
        {
            foreach (Tile tile in rooms[i])
            {
                tile.name = $"Room_{i + 1}";
            }
        }
    }

    private bool GrowRoomByOneTile(List<Tile> room, Color roomColor)
    {
        List<Tile> potentialNeighbors = new List<Tile>();
        foreach (Tile tile in room)
        {
            if (tile.neighborLeft != null && !IsAssigned(tile.neighborLeft)) potentialNeighbors.Add(tile.neighborLeft);
            if (tile.neighborRight != null && !IsAssigned(tile.neighborRight)) potentialNeighbors.Add(tile.neighborRight);
            if (tile.neighborUp != null && !IsAssigned(tile.neighborUp)) potentialNeighbors.Add(tile.neighborUp);
            if (tile.neighborDown != null && !IsAssigned(tile.neighborDown)) potentialNeighbors.Add(tile.neighborDown);
        }

        if (potentialNeighbors.Count > 0)
        {
            Tile newTile = potentialNeighbors[Random.Range(0, potentialNeighbors.Count)];
            room.Add(newTile);
            newTile.GetComponent<Renderer>().material.color = roomColor;
            return true;
        }

        return false;
    }

    private bool IsAssigned(Tile tile)
    {
        return tile.GetComponent<Renderer>().material.color != Color.white;
    }

    private Tile GetRandomUnassignedTile()
    {
        List<Tile> unassignedTiles = new List<Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] != null && !IsAssigned(grid[x, y]))
                {
                    unassignedTiles.Add(grid[x, y]);
                }
            }
        }

        if (unassignedTiles.Count > 0)
        {
            return unassignedTiles[Random.Range(0, unassignedTiles.Count)];
        }
        return null;
    }
}
















