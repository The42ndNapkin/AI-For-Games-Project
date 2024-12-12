using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 gridPos;
    private bool submerged = false;
    [SerializeField] private GenerateTiles tiles;
    private int height, width;
    private bool pTurn = true;

    void OnEnable()
    {
        tiles = GameObject.FindWithTag("Board").GetComponent<GenerateTiles>();
        gridPos = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(pTurn);
        //If it is player's turn, lets them move once based on player input, then passes turn to guards AI
        if(pTurn)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (moveRight())
                {
                    passTurn();
                }
                
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (moveLeft())
                {
                    passTurn();
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if(moveUp())
                {
                    passTurn();
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if(moveDown())
                {
                    passTurn();
                }
            }
            //If space key pressed, vent to other vent
            //Iterates over entire 2d array until 2 vector2s of vent locations
            //Move player to location that they aren't currently at
        }
        
    }

    //Sets the player position in world space given the grid position they should be in
    public void setWorldPos(Vector2 gridLoc)
    {
        Vector2 worldPos = new Vector2(gridLoc.y - 8.5f, 4.5f - gridLoc.x);
        gridPos = gridLoc;
        gameObject.transform.position = new Vector3(worldPos.x, worldPos.y, -1);
        tileCheck();
        //Call event to update enemy knowledge and any pathfinding
        
    }

    //Shifts player turn to guards AI
    public void passTurn()
    {
        //StartCoroutine(waitCoroutine(1));
        if(pTurn)
        {
            pTurn = false;
        }
        
    }

    //Shifts AI turn to player's turn
    public void recieveTurn()
    {
        //StartCoroutine(waitCoroutine(1));
        if(!pTurn)
        {
            pTurn = true;
        }
    }

    //Funciton to update member variables
    public void tileCheck()
    {
        //Checks if player is on a water tile and if so, makes them submerged in water
        if (tiles.gameBoard[(int)gridPos.x, (int)gridPos.y] == GenerateTiles.Tiles.Water)
        {
            submerged = true;
        }
        else
        {
            submerged = false;
        }
        if (tiles.gameBoard[(int)gridPos.x, (int)gridPos.y] == GenerateTiles.Tiles.Door)
        {
            //Player wins game
            tiles.playerWin();
        }
        if (tiles.gameBoard[(int)gridPos.x, (int)gridPos.y] == GenerateTiles.Tiles.NoiseTile)
        {
            //call Noise event
        }

    }

    public Vector2 getGridPos()
    { return gridPos; }
    public bool submerge()
    { return submerged; }
    public bool playerTurn()
    { return pTurn; }

    //function to move player right, returns true if the move was legal, and false if not
    public bool moveRight()
    {
        if(gridPos.y >= tiles.getWidth() - 1)
        {
            return false;
        }
        else
        {
            //Check if no wall is present to the right, and move player if so
            if (tiles.gameBoard[(int)gridPos.x,(int)gridPos.y + 1] != GenerateTiles.Tiles.Wall)
            {
                setWorldPos(new Vector2(gridPos.x, gridPos.y + 1));
                return true;
            }
            return false;
        }
    }
    //function to move player left, returns true if the move was legal, and false if not
    public bool moveLeft()
    {
        if (gridPos.y <= 0)
        {
            return false;
        }
        else
        {
            //Check if no wall is present to the left, and move player if so
            if (tiles.gameBoard[(int)gridPos.x, (int)gridPos.y - 1] != GenerateTiles.Tiles.Wall)
            {
                setWorldPos(new Vector2(gridPos.x, gridPos.y - 1));
                return true;
            }
            return false;
        }
    }
    //function to move player up, returns true if the move was legal, and false if not
    public bool moveUp()
    {
        if (gridPos.x <= 0)
        {
            return false;
        }
        else
        {
            //Check if no wall is present above player, and move player if so
            if (tiles.gameBoard[(int)gridPos.x - 1, (int)gridPos.y] != GenerateTiles.Tiles.Wall)
            {
                setWorldPos(new Vector2(gridPos.x - 1, gridPos.y));
                return true;
            }
            return false;
        }
    }
    //function to move player Down, returns true if the move was legal, and false if not
    public bool moveDown()
    {
        if (gridPos.x >= tiles.getHeight() - 1)
        {
            return false;
        }
        else
        {
            //Check if no wall is present above player, and move player if so
            if (tiles.gameBoard[(int)gridPos.x + 1, (int)gridPos.y] != GenerateTiles.Tiles.Wall)
            {
                setWorldPos(new Vector2(gridPos.x + 1, gridPos.y));
                return true;
            }
            return false;
        }
    }

    //Coroutine for int i as time delay
    IEnumerator waitCoroutine(int i)
    {
        yield return new WaitForSeconds(i);
    }
}
