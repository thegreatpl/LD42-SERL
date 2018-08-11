using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class StarSystem : MonoBehaviour {

    /// <summary>
    /// The main menu object. 
    /// </summary>
    public MenuController MainMenuScreen; 

    public Tilemap Tilemap;

    public Text Position;

    public Text Detail; 

    public Camera Camera;

    public GalaxyGenerator GalaxyGenerator;

    public Spritemanager Spritemanager;

    public PrefabManager PrefabManager; 




	// Use this for initialization
	void Start () {
        Tilemap = GetComponent<Tilemap>();
        GalaxyGenerator = GetComponent<GalaxyGenerator>();
        Spritemanager = GetComponent<Spritemanager>();
        PrefabManager = GetComponent<PrefabManager>(); 

        LoadMainMenu(); 
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

    /// <summary>
    /// Whether or not the tile is passable. 
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public bool IsPassable(Vector3Int tilePos)
    {
        if (Tilemap.HasTile(tilePos))
            return true;

        return false; 
    }

    /// <summary>
    /// Gets the movement cost of this tile. 
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public int GetMovementCost(Vector3Int tilePos)
    {
        if (!IsPassable(tilePos))
            return int.MaxValue;

        return ((SpaceTile)Tilemap.GetTile(tilePos))?.MovementCost ?? 1; 
    }


    /// <summary>
    /// Starts a new game. 
    /// </summary>
    public void StartNewGame()
    {
        GalaxyGenerator.GenerateGalaxy();
        LoadMainGameScreen(); 
    }

    /// <summary>
    /// Loads the in game screen. 
    /// </summary>
    public void LoadMainGameScreen()
    {
        MainMenuScreen.ClearMenu();
        MainMenuScreen.AddButton("mainmenu", "esc - Main Menu", LoadMainMenu, KeyCode.Escape); 
    }


    /// <summary>
    /// Loads the main menu screen. 
    /// </summary>
    public void LoadMainMenu()
    {
        MainMenuScreen.ClearMenu(); 
        MainMenuScreen.AddButton("newgame", "n - Start New Game", StartNewGame, KeyCode.N);
    }
}
