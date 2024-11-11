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

    private int height = 3;
    private int width = 3;

    private Tiles[,] gameBoard;

    // Start is called before the first frame update
    void Start()
    {
        
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
