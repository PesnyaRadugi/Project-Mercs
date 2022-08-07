using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    [SerializeField] internal TileBase[] tiles;

    [SerializeField] internal int movementCost;
}
