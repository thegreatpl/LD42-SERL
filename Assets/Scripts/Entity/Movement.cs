﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public StarSystem StarSystem; 

    public Vector3Int Location; 

	// Use this for initialization
	void Start () {
        Move(Location); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Move(PointyDirection direction)
    {
        Move(Location.GetVectorInDirection(direction)); 
    }

    /// <summary>
    /// Moves to the specified position. 
    /// </summary>
    /// <param name="newLoc"></param>
    public void Move(Vector3Int newLoc)
    {
        if (!StarSystem.IsPassable(newLoc))
            return; 

        var worldPos = StarSystem.TileToWorld(newLoc);
        transform.position = worldPos;
        Location = newLoc; 
    }
}
