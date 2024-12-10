using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTiles : MonoBehaviour
{
    public enum Tiles
    {
        UNDEFINED,
        Wall,
        Floor,
        Water,
        Door,
        Box,
        Vent,
        NoiseTile
    }

    [SerializeField] private List<Tile> tileSet;

    [SerializeField] InspectableDictionary iDic;
    private Dictionary<Tile, HashSet<Tile>> dict;

    [SerializeField] private int height = 10;
    [SerializeField] private int width = 18;

    public Tiles[,] gameBoard;

    // Start is called before the first frame update
    void Start()
    {
        gameBoard = new Tiles[height, width];
        PremakeMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Function to initialize the map's preset state
    public void PremakeMap()
    {
        //Breaks out of the function if the board space isn't exactly lining up with the preset
        if(height != 10 || width != 18)
        {
            Debug.LogWarning("Bounds of game board don't line up with the preset.");
            return;
        }
        //Setup the tile walls
        for(int i = 0; i < width; i++)
        {
            gameBoard[0, i] = Tiles.Wall;
        }
        for(int i = 0; i < width; i++)
        {
            gameBoard[height-1, i] = Tiles.Wall;
        }
        for(int i = 0; i < height; i++)
        {
            gameBoard[i,0] = Tiles.Wall;
        }
        for(int i = 0; i < height; i++)
        {
            gameBoard[i,width-1] = Tiles.Wall;
        }
        //Exit door
        gameBoard[1, 3] = Tiles.Door;
        //Vents
        gameBoard[2, 1] = Tiles.Vent;
        gameBoard[7, 15] = Tiles.Vent;
        //Remaining Walls
        gameBoard[1, 8] = Tiles.Wall;
        gameBoard[2, 8] = Tiles.Wall;
        gameBoard[3, 8] = Tiles.Wall;
        gameBoard[3,14] = Tiles.Wall;
        gameBoard[3,15] = Tiles.Wall;
        gameBoard[3,16] = Tiles.Wall;
        gameBoard[3,17] = Tiles.Wall;
        gameBoard[5,9] = Tiles.Wall;
        gameBoard[5,8] = Tiles.Wall;
        gameBoard[5,7] = Tiles.Wall;
        gameBoard[5,6] = Tiles.Wall;
        gameBoard[5,5] = Tiles.Wall;
        gameBoard[6,5] = Tiles.Wall;
        gameBoard[7,5] = Tiles.Wall;
        //Boxes
        gameBoard[2, 16] = Tiles.Box;
        gameBoard[2, 15] = Tiles.Box;
        gameBoard[2, 14] = Tiles.Box;
        gameBoard[5, 13] = Tiles.Box;
        gameBoard[5, 12] = Tiles.Box;
        gameBoard[5, 11] = Tiles.Box;
        gameBoard[5, 10] = Tiles.Box;
        gameBoard[6, 13] = Tiles.Box;
        gameBoard[6, 12] = Tiles.Box;
        gameBoard[6, 11] = Tiles.Box;
        gameBoard[6, 10] = Tiles.Box;
        gameBoard[4, 6] = Tiles.Box;
        gameBoard[4, 1] = Tiles.Box;
        gameBoard[5, 1] = Tiles.Box;
        //Water
        gameBoard[1, 6] = Tiles.Water;
        gameBoard[1, 7] = Tiles.Water;
        gameBoard[2, 6] = Tiles.Water;
        gameBoard[2, 7] = Tiles.Water;
        for(int i = 0; i < 8; i++)
        {
            gameBoard[1, 9+i] = Tiles.Water;
        }
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                gameBoard[6 + j,1 + i] = Tiles.Water;
            }
        }
        gameBoard[8, 5] = Tiles.Water;
        //Noise Tiles
        gameBoard[3, 3] = Tiles.NoiseTile;
        gameBoard[5, 2] = Tiles.NoiseTile;
        gameBoard[6, 6] = Tiles.NoiseTile;
        gameBoard[8, 7] = Tiles.NoiseTile;
        gameBoard[8, 13] = Tiles.NoiseTile;
        gameBoard[5, 14] = Tiles.NoiseTile;
        gameBoard[2, 12] = Tiles.NoiseTile;
        //The rest of the tiles are floors
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                if (gameBoard[i,j] == Tiles.UNDEFINED)
                {
                    gameBoard[i, j] = Tiles.Floor;
                }
            }
        }
    }

    //Function to create a game map using wave function collapse
    public void GenerateMap()
    {
        //Start by placing the exit on a random board edge
        int exitEdge = Random.Range(0, 4);
        int otherSide;
        switch(exitEdge)
        {
            case 0:
                otherSide = Random.Range(0, height);
                gameBoard[0, otherSide] = Tiles.Door;
                break;
            case 1:
                otherSide = Random.Range(0, height);
                gameBoard[width, otherSide] = Tiles.Door;
                break;
            case 2:
                otherSide = Random.Range(0, width);
                gameBoard[otherSide, 0] = Tiles.Door;
                break;
            case 3:
                otherSide = Random.Range(0, width);
                gameBoard[otherSide, height] = Tiles.Door;
                break;
            default:
                otherSide = Random.Range(0, width);
                gameBoard[otherSide, height] = Tiles.Door;
                break;
        }


    }

    private int calculateEntropy(int x, int y)
    {
        List<Tiles> nearbyTiles = new List<Tiles>();
        int count = 0;
        if(x < width-1)
        {
            nearbyTiles.Add(gameBoard[x + 1, y]);
            count++;
        }
        if(x > 0)
        {
            nearbyTiles.Add(gameBoard[x - 1, y]);
            count++;
        }
        if(y < height-1)
        {
            nearbyTiles.Add(gameBoard[x, y + 1]);
            count++;
        }
        if (y > 0)
        {
            nearbyTiles.Add(gameBoard[x, y - 1]);
            count++;
        }
        
        return (getSameTiles(nearbyTiles, count).Count);
    }

    private List<Tile> getSameTiles(List<Tiles> neighbors, int sides)
    {
        //Fill a list of tiles with all possible adjacent tiles from each tile in neighbors
        //The tiles that qualify for adjacency will appear in this list 4 times
        List<Tile> lineups = new List<Tile>();
        foreach(Tiles t in neighbors)
        {
            HashSet<Tile> h = dict[tileSet[(int)t - 1]];
            foreach(Tile add in h)
            {
                lineups.Add(add);
            }
        }
        //Fill a hashtable with all possible adjacent tiles, and give them an int value for how many times they've appeared in lineups
        Hashtable sameTiles = new Hashtable();
        foreach (Tile t in lineups)
        {
            if (!sameTiles.Contains(t))
            {
                sameTiles.Add(t, 1);
            }
            else
            {
                int c = (int)sameTiles[t];
                sameTiles.Remove(t);
                c++;
                sameTiles.Add(t, c);
            }
        }
        foreach(Tile t in lineups)
        {
            Debug.Log(t);
        }
        
        //Reset lineups to save memory
        lineups.Clear();
        foreach(object t in sameTiles.Keys)
        {
            if ((int)sameTiles[t] == sides)
            {
                lineups.Add((Tile)t);
            }
        }
        return lineups;
    }
}
