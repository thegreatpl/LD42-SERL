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
    /// The name of this tile, if any. 
    /// </summary>
    public string Name = "";

    /// <summary>
    /// How much this tile is worth in minerals. 
    /// </summary>
    public int MineralValue = 0;

    /// <summary>
    /// how much this tile costs to move. 
    /// </summary>
    public int MovementCost = 1;

    /// <summary>
    /// Region of the tile. 
    /// </summary>
    public string Region = ""; 


    public void FromTile(SpaceTile t)
    {
        Name = t.Name;
        MovementCost = t.MovementCost;
        Region = t.Region;
        sprite = t.sprite;
        color = t.color;
        MineralValue = t.MineralValue; 

    }

}

