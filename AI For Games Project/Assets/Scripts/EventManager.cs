using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Start is called before the first frame update
    public delegate void movePlayer(Vector2 gridLoc);
    public static event movePlayer move;
    
    public void callMovePlayer(Vector2 gridLoc)
    {
        move(gridLoc);
    }
}
