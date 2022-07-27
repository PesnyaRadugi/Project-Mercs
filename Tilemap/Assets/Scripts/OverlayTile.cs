using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    internal int G;
    internal int H;
    internal int F{ get {return G + H; } }
    internal bool isBlocked = false;
    internal OverlayTile previousTile;


    public Vector3Int gridLocation;

    
    private void Update() 
    {
        if (Input.GetMouseButtonDown(1))
        {
            HideTile();
        }    
    }

    internal void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    internal void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

}

