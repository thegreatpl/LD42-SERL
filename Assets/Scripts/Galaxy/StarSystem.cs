using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class StarSystem : MonoBehaviour {

    public Tilemap Tilemap;

    public Text Position;

    public Camera Camera;



	// Use this for initialization
	void Start () {
        Tilemap = GetComponent<Tilemap>();



	}
	
	// Update is called once per frame
	void Update () {
        var mousePos = Input.mousePosition;

        var worldPos = Camera.ScreenToWorldPoint(mousePos);
        var tile = WorldToTile(worldPos);
        Position.text = tile.x + ", " + tile.y + ", " + tile.z;

	}

    public Vector3Int WorldToTile(Vector3 pos)
    {
        return Tilemap.WorldToCell(pos); 
    }

    public Vector3 TileToWorld(Vector3Int tilePos)
    {
        return Tilemap.CellToWorld(tilePos); 
    }


    public bool IsPassable(Vector3Int tilePos)
    {
        if (Tilemap.HasTile(tilePos))
            return true;

        return false; 
    }
}
