using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class StarSystem : MonoBehaviour {

    public Tilemap Tilemap;

    public Text Position;

    public Camera Camera;

    public GameObject debugObj;

    GameObject[] debug;

	// Use this for initialization
	void Start () {
        Tilemap = GetComponent<Tilemap>();

        debug = new GameObject[6]; 
        for(int idx = 0; idx < 6; idx++)
        {
            debug[idx] = Instantiate(debugObj); 
        }

	}
	
	// Update is called once per frame
	void Update () {
        var mousePos = Input.mousePosition;

        var worldPos = Camera.ScreenToWorldPoint(mousePos);
        var tile = ToTilePosition(worldPos);
        Position.text = tile.x + ", " + tile.y + ", " + tile.z;

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            for (int idx = 0; idx < 6; idx++)
            {
                //var direct = Hex.Direction(idx);
                //var vect2 = OffsetCoord.RToUnityCoords(direct);
                //debug[idx].transform.position = TileToWorld(vect2);
                //debug[idx].name = direct.q + ", " + direct.r + ", " + direct.s;



                var tilePos = tile.GetVectorInDirection((PointyDirection)(idx));
                debug[idx].transform.position = TileToWorld(tilePos);
                debug[idx].name = tilePos.ToString() + ":" + ((PointyDirection)idx).ToString();
            }
        }
	}

    public Vector3Int ToTilePosition(Vector3 pos)
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
