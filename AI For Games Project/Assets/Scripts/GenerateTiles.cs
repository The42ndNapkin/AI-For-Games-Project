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
}
