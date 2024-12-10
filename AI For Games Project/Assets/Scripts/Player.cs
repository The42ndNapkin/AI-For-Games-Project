using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 gridPos;
    bool turn;
    bool submerged;
    [SerializeField] private GenerateTiles tiles;
    private GenerateTiles.Tiles[,] gameBoard;

    // Start is called before the first frame update
    void Start()
    {
        //Copy the board state
        gameBoard = tiles.gameBoard;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Sets the player position in world space given the grid position they should be in
    public void setWorldPos(Vector2 gridLoc)
    {
        Vector2 worldPos = new Vector2(gridLoc.x - 8.5f, gridLoc.y + 4.5f);
    }
}
