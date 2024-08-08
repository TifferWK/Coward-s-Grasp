using UnityEngine;

public class TileGridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;  // Assign the Tile prefab in the Unity inspector
    public int width = 6;
    public int height = 7;
    public float tileSpacing = 1.0f;  // Adjust spacing between tiles if needed

    private Tile[,] grid;

    void Start()
    {
        GenerateTileGrid();
    }

    void GenerateTileGrid()
    {
        grid = new Tile[width, height];

        // Instantiate and arrange tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Set position so that tiles form a flat grid
                Vector3 position = new Vector3(x * tileSpacing, 0, y * tileSpacing);
                GameObject tileObject = Instantiate(tilePrefab, position, Quaternion.identity);
                tileObject.transform.parent = transform;  // Set parent for organization

                Tile tile = tileObject.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.position = new Vector2Int(x, y);
                    grid[x, y] = tile;
                }
            }
        }

        // Set neighbors for each tile
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
}










