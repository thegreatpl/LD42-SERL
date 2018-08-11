using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;

[Serializable]
public class SpaceTile : Tile
{
    /// <summary>
    /// how much this tile costs to move. 
    /// </summary>
    public int MovementCost = 1; 

}

