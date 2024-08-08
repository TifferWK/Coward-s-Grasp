using UnityEngine;

public class Tile : MonoBehaviour
{
    // Cardinal neighbors
    public Tile neighborLeft;
    public Tile neighborRight;
    public Tile neighborUp;
    public Tile neighborDown;

    // Ordinal neighbors
    public Tile topLeft;
    public Tile topRight;
    public Tile bottomLeft;
    public Tile bottomRight;

    // Optionally, you might want to store additional information
    // about the tile, like its position or type.
    public Vector2Int position;  // Example position in a grid

    // Set all neighbors
    public void SetNeighbors(Tile left, Tile right, Tile up, Tile down,
                             Tile tl, Tile tr, Tile bl, Tile br)
    {
        neighborLeft = left;
        neighborRight = right;
        neighborUp = up;
        neighborDown = down;
        topLeft = tl;
        topRight = tr;
        bottomLeft = bl;
        bottomRight = br;
    }

    // Method to get all neighbors
    public Tile[] GetNeighbors()
    {
        return new Tile[] { neighborLeft, neighborRight, neighborUp, neighborDown, topLeft, topRight, bottomLeft, bottomRight };
    }

    // Optionally, you can add methods to interact with neighbors
    public void PrintNeighborPositions()
    {
        Debug.Log("Left neighbor position: " + (neighborLeft ? neighborLeft.position.ToString() : "None"));
        Debug.Log("Right neighbor position: " + (neighborRight ? neighborRight.position.ToString() : "None"));
        Debug.Log("Up neighbor position: " + (neighborUp ? neighborUp.position.ToString() : "None"));
        Debug.Log("Down neighbor position: " + (neighborDown ? neighborDown.position.ToString() : "None"));
        Debug.Log("TopLeft neighbor position: " + (topLeft ? topLeft.position.ToString() : "None"));
        Debug.Log("TopRight neighbor position: " + (topRight ? topRight.position.ToString() : "None"));
        Debug.Log("BottomLeft neighbor position: " + (bottomLeft ? bottomLeft.position.ToString() : "None"));
        Debug.Log("BottomRight neighbor position: " + (bottomRight ? bottomRight.position.ToString() : "None"));
    }
}



