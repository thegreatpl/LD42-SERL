using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GalaxyGenerator : MonoBehaviour {

    public Tilemap Tilemap;

    public TileBase EmptySpace;

    public StarSystem StarSystem;

    public Canvas WorldCanvas;

    public Camera Camera; 

	// Use this for initialization
	void Start () {
        Tilemap = GetComponent<Tilemap>();
        StarSystem = GetComponent<StarSystem>(); 
        GenerateGalaxy(25, new Vector3Int(0, 0, 0)); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateGalaxy(int radius, Vector3Int start)
    {

        var debugObj = WorldCanvas.GetComponentInChildren<Text>().gameObject; 
        //debugObj.AddComponent<Text>(); 
        //debugObj.GetComponent<Text>().fontSize = 1; 
        
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Math.Max(-radius, -q - radius);
            int r2 = Math.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                var hex = new Hex(q, r);
                var offset = OffsetCoord.RToUnityCoords(hex); 

                var tilePos = new Vector3Int(start.x + offset.x, start.y + offset.y, 0);
                var obj = Instantiate(debugObj);
                obj.transform.position = StarSystem.TileToWorld(tilePos);

                obj.GetComponent<Text>().text = hex.q + ", " + hex.r;
                obj.transform.SetParent(WorldCanvas.transform, true);
                obj.transform.localScale = new Vector3(1, 1, 1);


                Tilemap.SetTile(tilePos, EmptySpace);
            }
        }
    }
}
