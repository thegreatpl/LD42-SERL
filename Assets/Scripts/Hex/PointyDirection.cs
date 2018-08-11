using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum PointyDirection
    {
        Current = -1,
        NorthEast = 1,
        East = 0,
        SouthEast = 5,
        SouthWest = 4,
        West = 3, 
        NorthWest = 2
    }


public static partial class DirectionExtensions
{
    public static Vector3Int GetVectorInDirection(this Vector3Int pos, PointyDirection pointyDirection)
    {
        var hex = OffsetCoord.RFromUnity(pos);
        var hex2 = OffsetCoord.RoffsetToCube(new OffsetCoord(pos.y, pos.x)); 
        var neighbour = hex.Neighbor((int)pointyDirection);

        var result2 = OffsetCoord.RoffsetFromCube(neighbour);
        var result1 = OffsetCoord.RToUnityCoords(neighbour);
        return result1; 
    }
}

