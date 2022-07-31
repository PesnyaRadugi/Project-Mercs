using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    //Pathfndig
    internal int gCost;
    internal int hCost;
    internal int fCost{ get {return gCost + hCost; } }
    internal bool isBlocked = false;
    internal PathNode previousNode;

    //Coordinates
    internal Vector3Int gridLocation;

    
    private void Update() 
    {
        if (Input.GetMouseButtonDown(1))
        {
            HideNode();
        }    
    }

    internal void HideNode()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    internal void ShowNode()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

}

