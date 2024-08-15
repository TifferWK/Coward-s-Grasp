
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


//CSW, 2024. 
//'Civilization' type dungeon generation. 
//flood fill type generation... 

public class TileGridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject wallPrefab;
    public GameObject doorPrefab;
    public int width = 6;
    public int height = 7;
    public float tileSpacing = 1.0f;

    public int RandomTiles = 3;
    public Material roomMaterial;   //frankly this is just a dummy texture... ideally you'd axe this in proper implementation, I just needed a debug viz... 

    private Dictionary<Vector3, GameObject> wallDictionary = new Dictionary<Vector3, GameObject>();
    private Dictionary<List<Tile>, List<List<Tile>>> roomGraph = new Dictionary<List<Tile>, List<List<Tile>>>();

    private Tile[,] grid;
    private List<List<Tile>> rooms;

        
    
    void Start()
    {
        //in plain english, first you make the grid, then you select random tiles in the grid, then you search for neighbors of the random tiles and add them to the "room" the tile makes.
        GenerateTileGrid();
        DetermineRooms();
        GrowRoomsAlternately();
        foreach (Tile tile in grid)
        {
            CheckIfTileIsExcrescentFromItsRoom(tile);
        }
        MakeWallsFromRooms();
        MakeDoor();
        EnsureRoomConnectivity();
    }

    #region MAKE_THE_GRID
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
                tileObject.transform.parent = transform;  

                Tile tile = tileObject.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.position = new Vector2Int(x, y);
                    grid[x, y] = tile;
                }
            }
        }

        //assign neighbors to the tile class associated with each tile... 
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
    #endregion MAKE_THE_GRID    

    #region START_FIGURING_OUT_WHERE_THE_ROOMS_BEGIN
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
    #endregion START_FIGURING_OUT_WHERE_THE_ROOMS_BEGIN 

    #region     HELPER_METHOD_FOR_GRID_TO_TELL_TILES_WHOSE_NEIGHBOR_IS_WHOSE
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
    #endregion HELPER_METHOD_FOR_GRID_TO_TELL_TILES_WHOSE_NEIGHBOR_IS_WHOSE


    #region MAKE_THE_ROOMS
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

    //this frankly warrants a rewrite or break into it's own generation scheme.
    //I think that this could be written instead where I scroll through the whole of the grid and all neighbors to a colored tile get added to the 
    //colored tile, it would check if it's got a colored neighbor, and will decide what color to choose based off of either 
    //a). there are 3 room_a tiles as neighbors and 1 room_b tiles as neighbors-- it will choose the smaller number (probably would make cool hallways)
    //or if it picks the bigger number, you'd have more boxy rooms I imagine. I'd need to test it... 
    //b). select the first room_n found in the neighbors... idk I'll play with it when I have more time... 

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

    //probably should move "is assigned" to the tile class but I'm going to hold off on that until I have the wall solver... 
    //may need to be more 'semioticially rich' in a sense, a single bool might cause me issues later.. .
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
    #endregion MAKE_THE_ROOMS

    #region EXCRESCENCECHECK
    public void CheckIfTileIsExcrescentFromItsRoom(Tile tile)
    {
        int sameRoomNeighborCount = 0;

        //check by counting off...
        if (IsInSameRoom(tile, tile.neighborLeft)) sameRoomNeighborCount++;
        if (IsInSameRoom(tile, tile.neighborRight)) sameRoomNeighborCount++;
        if (IsInSameRoom(tile, tile.neighborUp)) sameRoomNeighborCount++;
        if (IsInSameRoom(tile, tile.neighborDown)) sameRoomNeighborCount++;

        // If only one neighbor is in the same room, it's an excrescent tile
        if (sameRoomNeighborCount == 1)
        {
            Debug.Log($"{tile.name} is an excrescent tile.");
            tile.isExcresent = true;
        }
        else
        {
            Debug.Log($"{tile.name} is not an excrescent tile.");
            tile.isExcresent = false;
        }
    }

    private bool IsInSameRoom(Tile tile, Tile neighbor)
    {
        if (neighbor == null) return false;

        // Check if the neighbor is in the same room as the tile
        foreach (List<Tile> room in rooms)
        {
            if (room.Contains(tile) && room.Contains(neighbor))
            {
                return true;
            }
        }
        return false;
    }
    #endregion EXCRESCENCECHECK

    #region WALLS_AND_DOORS
    public void MakeWallsFromRooms()
    {
        HashSet<Vector3> wallPositions = new HashSet<Vector3>();

        foreach (List<Tile> room in rooms)
        {
            foreach (Tile tile in room)
            {
                CreateWallIfNecessary(tile, tile.neighborLeft, Vector3.left, wallPositions);
                CreateWallIfNecessary(tile, tile.neighborRight, Vector3.right, wallPositions);
                CreateWallIfNecessary(tile, tile.neighborUp, Vector3.forward, wallPositions);
                CreateWallIfNecessary(tile, tile.neighborDown, Vector3.back, wallPositions);
            }
        }
    }

    private void CreateWallIfNecessary(Tile tile, Tile neighbor, Vector3 direction, HashSet<Vector3> wallPositions)
    {
        if (neighbor == null || !CheckIsInSameRoom(tile, neighbor))
        {
            Vector3 wallPosition = tile.transform.position + (direction * (tileSpacing / 2));
            wallPosition.y += 0.5f; 

            if (!wallPositions.Contains(wallPosition))
            {
                GameObject wall = Instantiate(wallPrefab, wallPosition, Quaternion.identity);
                wall.transform.parent = transform;

                if (direction == Vector3.left || direction == Vector3.right)
                {
                    wall.transform.Rotate(0, 90, 0);
                }

                wallPositions.Add(wallPosition);
                wallDictionary[wallPosition] = wall; 
            }
        }
    }

    private bool CheckIsInSameRoom(Tile tile, Tile neighbor)
    {
        if (neighbor == null) return false;

       
        foreach (List<Tile> room in rooms)
        {
            if (room.Contains(tile) && room.Contains(neighbor))
            {
                return true;
            }
        }
        return false;
    }


    private void MakeDoor()
    {
        HashSet<List<Tile>> roomsWithDoors = new HashSet<List<Tile>>();

        foreach (List<Tile> room in rooms)
        {
            bool doorCreated = false;

            //go for the excresent tile first... 
            foreach (Tile tile in room)
            {
                if (tile.isExcresent && !roomsWithDoors.Contains(room))
                {
                    if (CreateDoorIfNecessary(tile, tile.neighborLeft, Vector3.left, room) ||
                        CreateDoorIfNecessary(tile, tile.neighborRight, Vector3.right, room) ||
                        CreateDoorIfNecessary(tile, tile.neighborUp, Vector3.forward, room) ||
                        CreateDoorIfNecessary(tile, tile.neighborDown, Vector3.back, room))
                    {
                        roomsWithDoors.Add(room);
                        doorCreated = true;
                        break; 
                    }
                }
            }

            //for no excresent tile cases... 
            if (!doorCreated)
            {
                foreach (Tile tile in room)
                {
                    if (!roomsWithDoors.Contains(room))
                    {
                        if (CreateDoorIfNecessary(tile, tile.neighborLeft, Vector3.left, room) ||
                            CreateDoorIfNecessary(tile, tile.neighborRight, Vector3.right, room) ||
                            CreateDoorIfNecessary(tile, tile.neighborUp, Vector3.forward, room) ||
                            CreateDoorIfNecessary(tile, tile.neighborDown, Vector3.back, room))
                        {
                            roomsWithDoors.Add(room);
                            break; 
                        }
                    }
                }
            }
        }
    }

    private bool CreateDoorIfNecessary(Tile tile, Tile neighbor, Vector3 direction, List<Tile> room)
    {
        if (neighbor != null && !IsInSameRoom(tile, neighbor))
        {
            Vector3 doorPosition = tile.transform.position + (direction * (tileSpacing / 2));
            doorPosition.y += 0.5f;

            //get rid of intersecting walls... 
            if (wallDictionary.ContainsKey(doorPosition))
            {
                Destroy(wallDictionary[doorPosition]);
                wallDictionary.Remove(doorPosition);
            }

            GameObject door = Instantiate(doorPrefab, doorPosition, Quaternion.identity);
            door.transform.parent = transform;

            if (direction == Vector3.left || direction == Vector3.right)
            {
                door.transform.Rotate(0, 90, 0);
            }

            return true; //door got... 
        }

        return false; //no door.. 
    }
    private void EnsureRoomConnectivity()
    {
        while (!AreAllRoomsConnected())
        {
            //search for unconnected rooms and make a door between them... 
            for (int i = 0; i < rooms.Count; i++)
            {
                for (int j = i + 1; j < rooms.Count; j++)
                {
                    if (!AreRoomsConnected(rooms[i], rooms[j]))
                    {
                        CreateDoorBetweenRooms(rooms[i], rooms[j]);
                        break;
                    }
                }
            }
        }
    }

    private bool AreRoomsConnected(List<Tile> room1, List<Tile> room2)
    {
        HashSet<Tile> visitedTiles = new HashSet<Tile>();
        Queue<Tile> queue = new Queue<Tile>();

        foreach (Tile tile in room1)
        {
            queue.Enqueue(tile);
            visitedTiles.Add(tile);
        }

        while (queue.Count > 0)
        {
            Tile currentTile = queue.Dequeue();

            foreach (Tile neighbor in new Tile[] { currentTile.neighborLeft, currentTile.neighborRight, currentTile.neighborUp, currentTile.neighborDown })
            {
                if (neighbor != null)
                {
                    if (room2.Contains(neighbor))
                        return true;

                    if (!visitedTiles.Contains(neighbor))
                    {
                        visitedTiles.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        return false;
    }

    private void CreateDoorBetweenRooms(List<Tile> room1, List<Tile> room2)
    {
        foreach (Tile tile in room1)
        {
            foreach (Tile neighbor in new Tile[] { tile.neighborLeft, tile.neighborRight, tile.neighborUp, tile.neighborDown })
            {
                if (neighbor != null && room2.Contains(neighbor))
                {
                    Vector3 direction = neighbor.transform.position - tile.transform.position;
                    CreateDoorIfNecessary(tile, neighbor, direction.normalized, room1);
                    return;
                }
            }
        }
    }
    private bool AreAllRoomsConnected()
    {
        if (rooms == null || rooms.Count == 0)
            return true;

        HashSet<List<Tile>> visitedRooms = new HashSet<List<Tile>>();
        Queue<List<Tile>> queue = new Queue<List<Tile>>();

        //first room... 
        queue.Enqueue(rooms[0]);
        visitedRooms.Add(rooms[0]);

        while (queue.Count > 0)
        {
            List<Tile> currentRoom = queue.Dequeue();

            foreach (Tile tile in currentRoom)
            {
                foreach (Tile neighbor in new Tile[] { tile.neighborLeft, tile.neighborRight, tile.neighborUp, tile.neighborDown })
                {
                    if (neighbor != null)
                    {
                        List<Tile> neighborRoom = rooms.FirstOrDefault(r => r.Contains(neighbor));
                        if (neighborRoom != null && !visitedRooms.Contains(neighborRoom))
                        {
                            visitedRooms.Add(neighborRoom);
                            queue.Enqueue(neighborRoom);
                        }
                    }
                }
            }
        }

        return visitedRooms.Count == rooms.Count;
    }


    #endregion WALLS

}
















