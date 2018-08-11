using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq; 

public class GalaxyGenerator : MonoBehaviour {

    public Tilemap Tilemap;



    public StarSystem StarSystem;

    public Spritemanager Spritemanager;


    public int GalaxyRadius = 100;

    public int StarSystemAttempts = 40;

    public int MinPlanets = 2; 

    public int MaxPlanets = 12; 

    #region tileTypes
    public TileBase EmptySpace;

    public List<TileBase> Star;

    public TileBase Planet; 

    #endregion
    // Use this for initialization
    void Start () {
        Tilemap = GetComponent<Tilemap>();
        StarSystem = GetComponent<StarSystem>();
        Spritemanager = GetComponent<Spritemanager>(); 

	}
	
	// Update is called once per frame
	void Update () {
		
	}



    /// <summary>
    /// Generates the galaxy. 
    /// </summary>
    public void GenerateGalaxy()
    {
        Tilemap.ClearAllTiles();
        LoadGraphics();
        GenerateMap(100); 
    }

    /// <summary>
    /// Loads the graphics in. 
    /// </summary>
    public void LoadGraphics()
    {
        EmptySpace = new SpaceTile() { color = Spritemanager.Colors["GREY"], sprite = Spritemanager.GetSprite("space") };
        Star = new List<TileBase>();
        Star.Add(new SpaceTile()
        {
            color = Spritemanager.Colors["YELLOW"],
            sprite = Spritemanager.GetSprite("star1"),
            MovementCost = 3
        }); 
        Star.Add(new SpaceTile()
        {
            color = Spritemanager.Colors["RED"],
                sprite = Spritemanager.GetSprite("star1"),
                MovementCost = 3
        });
        //Star.Add(new SpaceTile()
        //{
        //    color = Spritemanager.Colors["BROWN"],
        //    sprite = Spritemanager.GetSprite("star1"),
        //    MovementCost = 3
        //});

        Planet = new SpaceTile()
        {
            color = Spritemanager.Colors["BROWN"],
            sprite = Spritemanager.GetSprite("planet1"),
            MovementCost = 2
        };

    }

    void GenerateMap(int radius)
    {
        var galaxy = GetHexagon(radius, new Vector3Int(0, 0, 0));
        //populate the base galaxy. 
        foreach(var t in galaxy)
            Tilemap.SetTile(t, EmptySpace);

        var claimed = new List<Vector3Int>();


        for (int idx = 0; idx < StarSystemAttempts; idx++)
        {
            var star = galaxy.Random();
            if (claimed.Contains(star))
                continue;

            Tilemap.SetTile(star, Star.Random());

            int maxRadius = UnityEngine.Random.Range(MinPlanets, MaxPlanets);
            var c = OffsetCoord.RFromUnity(star); 
            for (int jdx = 1; jdx < maxRadius; jdx++)
            {
                var ring = GetRing(c, jdx);
                var ringAct = ring.Select(x => OffsetCoord.RToUnityCoords(x)).ToList();



                if (claimed.Intersect(ringAct).Any())
                    break;
                //keep everything in the galaxy. 
                if (ringAct.Count() != galaxy.Intersect(ringAct).Count())
                    break; 

                Tilemap.SetTile(ringAct.Random(), Planet); 
                claimed.AddRange(ringAct); 
            }

        }




    }

    /// <summary>
    /// Gets a hexagon. 
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="start"></param>
    /// <returns></returns>
    public static List<Vector3Int> GetHexagon(int radius, Vector3Int start)
    {
        var result = new List<Vector3Int>(); 
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Math.Max(-radius, -q - radius);
            int r2 = Math.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                var hex = new Hex(q, r);
                var offset = OffsetCoord.RToUnityCoords(hex);

                var tilePos = new Vector3Int(start.x + offset.x, start.y + offset.y, 0);
                result.Add(tilePos); 
            }
        }
        return result; 
    }

    /// <summary>
    /// Gets a ring around a specific point. 
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static List<Hex> GetRing(Hex center, int radius)
    {
        if (radius > 0)
        {
            List<Hex> results = new List<Hex>();
            Hex hex = Hex.Neighbor( center, 4, radius);

            for (int idx = 0; idx < 6; idx++)
            {
                for (int jdx = 0; jdx < radius; jdx++)
                {
                    results.Add(hex);
                    hex = hex.Neighbor(idx);
                }
            }


            return results;
        }
        else
        {
            return new List<Hex>() { center };
        }
    }


}
