using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTiles : MonoBehaviour
{
    enum Tiles
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

    private int height = 2;
    private int width = 2;

    private Tiles[,] gameBoard;

    // Start is called before the first frame update
    void Start()
    {
        gameBoard = new Tiles[width, height];
        dict = iDic.toDictionary();
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                gameBoard[i, j] = Tiles.Wall;
            }
        }
        Debug.Log(calculateEntropy(0, 0));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateMap()
    {

    }

    private int calculateEntropy(int x, int y)
    {
        List<Tiles> nearbyTiles = new List<Tiles>();
        if(x < width)
        {
            nearbyTiles.Add(gameBoard[x + 1, y]);
        }
        if(x > 0)
        {
            nearbyTiles.Add(gameBoard[x - 1, y]);
        }
        if(y < height)
        {
            nearbyTiles.Add(gameBoard[x, y + 1]);
        }
        if (y > 0)
        {
            nearbyTiles.Add(gameBoard[x, y - 1]);
        }
        
        return (getSameTiles(nearbyTiles).Count);
    }

    private List<Tile> getSameTiles(List<Tiles> neighbors)
    {
        //Fill a list of tiles with all possible adjacent tiles from each tile in neighbors
        //The tiles that qualify for adjacency will appear in this list 4 times
        List<Tile> lineups = new List<Tile>();
        foreach(Tiles t in neighbors)
        {
            HashSet<Tile> h = dict[tileSet[(int)t]];
            foreach(Tile add in h)
            {
                lineups.Add(add);
            }
        }
        //Fill a hashtable with all possible adjacent tiles, and give them an int value for how many times they've appeared in lineups
        Hashtable sameTiles = new Hashtable();
        foreach(Tile t in lineups)
        {
            if(!sameTiles.Contains(t))
            {
                sameTiles.Add(t,1);
            }
            else
            {
                int c = (int)sameTiles[t];
                sameTiles.Remove(t);
                c++;
                sameTiles.Add(t, c);
            }
        }
        lineups.Clear();
        foreach(Tile t in sameTiles)
        {
            lineups.Add(t);
        }
        return lineups;
    }
}
