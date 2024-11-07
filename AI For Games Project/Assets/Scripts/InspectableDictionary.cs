using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]

public struct WFCNode
{
    public Tile Key;
    public List<Tile> Values;
}

// serializable dictionary
public class InspectableDictionary : MonoBehaviour
{
    // trick the unity to show your data as a dictionary.
    // unity dont have a nice way to show dictionaries on the inspector
    [SerializeField] public List<WFCNode> Nodes;

    // use this to get the dictionary out of the nodes, in order to use it in your algorithms
    public Dictionary<Tile, HashSet<Tile>> toDictionary()
    {
        Dictionary<Tile, HashSet<Tile>> ret = new Dictionary<Tile, HashSet<Tile>>();

        foreach (var entry in Nodes)
        {
            if (!ret.ContainsKey(entry.Key))
            {
                ret.Add(entry.Key, new HashSet<Tile>());
            }

            foreach (var go in entry.Values)
            {
                ret[entry.Key].Add(go);
            }
        }

        return ret;
    }
}
