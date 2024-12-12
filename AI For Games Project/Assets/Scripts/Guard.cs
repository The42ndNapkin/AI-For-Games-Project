using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Guard : MonoBehaviour
{

    //behavior-related variables
    private bool submerged = false; //True if guard is in water
    
    //private Vector2 lastKnownLoc; //Stores last known location of the player
    private bool patrolDirection = true; //Direction is true if vertical, false if horizontal
    private int patrolLength = 2; //Half the patrol distance of the guard before turning around

    private bool knowsWherePlayer = false; //True if guard sees player or player hit a noise tile
    private Vector2 gridPos; //Grid location of the guard
    private Vector2 patrolLoc; //Grid location of current patrol center
    private Vector2 ogPatrolLoc; //Grid location of original patrol center
    private DecisionTree dTree; //Variable for decision tree
    public Pathfind pathfinder; //Variable for astar pathfinding
    public GenerateTiles tiles; //Used to access the gameBoard tiles
    private BigGrid gMoney; //Used to access the astar grid
    private bool noiseTile = false;  //Bool if noise tile has been set off
    private Player player;

    void OnEnable()
    {
        tiles = GameObject.FindWithTag("Board").GetComponent<GenerateTiles>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        //Copy the board state
        gridPos = new Vector2(0, 0);
        patrolLoc = gridPos;
        ogPatrolLoc = gridPos;
        pathfinder = gameObject.GetComponent<Pathfind>();
        dTree = gameObject.GetComponent<DecisionTree>();
        dTree.setRoot(new SelectorNode(gameObject.GetComponent<Guard>(), player));
        gMoney = gameObject.GetComponent<BigGrid>();
        gMoney.createGrid(tiles);
        pathfinder.bigGrid = gMoney;
    }

    public void setPatrolLength(int i)
    { patrolLength = i; }
    public void setPatrol(Vector2 loc)
    { patrolLoc = loc; }
    public Vector2 getPatrolLoc()
    { return patrolLoc; }
    public Vector2 getOGPatrol()
    { return ogPatrolLoc; }
    public Vector2 getGridPos()
    { return gridPos; }
    public bool submerge()
    { return submerged; }
    public List<Vector2> updateAStarGrid()
    { return(pathfinder.findPath(gridPos, player.getGridPos())); }
    public bool getPatrolDirection()
    { return patrolDirection; }
    public int getPatrolLength()
    { return patrolLength; }
    public bool doesKnowPlayer()
    { return knowsWherePlayer; }
    public void noiseOff()
    { noiseTile = false; }
    public void setPatrolLoc(Vector2 v)
    { patrolLoc = v; }

    // Update is called once per frame
    void Update()
    {
        //If it's the guards turn, take one action based off the decision tree AI and pass turn back to player
        if(!player.playerTurn())
        {
            //Update new guard knowledge based on player and board state
            updateInfo();
            //decision tree make a decision
            doBehavior();
            //Pass turn afterwards
        }
    }

    //Function to make guard do an action
    void doBehavior()
    {
        dTree.Execute();
        player.recieveTurn();
    }

    //Decision tree function to update guard behavior
    void updateInfo()
    {
        //Sets off noise tile if player stepped on it
        if(tiles.gameBoard[(int)player.getGridPos().x,(int)player.getGridPos().y] == GenerateTiles.Tiles.NoiseTile)
        {
            noiseTile = true;
        }
        //updates if player is visible to guard or if player stepped on noise tile
        if (canSeePlayer() || noiseTile)
        { knowsWherePlayer = true; }
        else { knowsWherePlayer = false; }
        
    }

    //returns true if the player is within 2 blocks of the guard and isn't hidden behind the wall or a box
    public bool canSeePlayer()
    {
        if (player.getGridPos().x <= gridPos.x + 2 && player.getGridPos().x >= gridPos.x - 2 && player.getGridPos().y <= gridPos.y + 2 && player.getGridPos().y >= gridPos.y - 2)
        {
            //Check if wall, box, or water is blocking view
            if (submerged == true && player.submerge() == true)
            {
                
                //Get a list of all the spaces in the a* chase path
                List<Vector2> astarPath = pathfinder.findPath(gridPos, player.getGridPos());
                
                //If list contains a box or a wall, or is more than 4 blocks, enemy cannot see player
                if (astarPath.Count >= 4)
                {
                    return false;
                }
                foreach (Vector2 v in astarPath)
                {
                    if (tiles.gameBoard[(int)v.x, (int)v.y] == GenerateTiles.Tiles.Wall || tiles.gameBoard[(int)v.x, (int)v.y] == GenerateTiles.Tiles.Box)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }

    //Sets the npc position in world space given the grid position they should be in
    public void setWorldPos(Vector2 gridLoc)
    {
        Vector2 worldPos = new Vector2(gridLoc.y - 8.5f, 4.5f - gridLoc.x);
        gridPos = gridLoc;
        gameObject.transform.position = new Vector3(worldPos.x, worldPos.y, -1);
    }
    //function to move npc right, returns true if the move was legal, and false if not
    public bool moveRight()
    {
        if (gridPos.y >= tiles.getWidth() - 1)
        {
            return false;
        }
        else
        {
            //Check if no wall is present to the right, and move player if so
            if (tiles.gameBoard[(int)gridPos.x, (int)gridPos.y + 1] != GenerateTiles.Tiles.Wall)
            {
                setWorldPos(new Vector2(gridPos.x, gridPos.y + 1));
                return true;
            }
            return false;
        }
    }
    //function to move npc left, returns true if the move was legal, and false if not
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
    //function to move npc up, returns true if the move was legal, and false if not
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
    //function to move npc down, returns true if the move was legal, and false if not
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
}
