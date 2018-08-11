using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class StarSystem : MonoBehaviour {

    public int EmpireNo = 20; 

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

    public TimeController TimeController;

    public EntityManager EntityManager;

    public EmpireManager EmpireManager; 

    public GameObject Cursor; 


    

	// Use this for initialization
	void Start () {
        Tilemap = GetComponent<Tilemap>();
        GalaxyGenerator = GetComponent<GalaxyGenerator>();
        Spritemanager = GetComponent<Spritemanager>();
        PrefabManager = GetComponent<PrefabManager>();
        TimeController = GetComponent<TimeController>();
        EntityManager = GetComponent<EntityManager>();
        EmpireManager = GetComponent<EmpireManager>(); 

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

    public SpaceTile GetSpaceTile(Vector3Int tilePos)
    {
        return (SpaceTile)Tilemap.GetTile(tilePos); 
    }


    /// <summary>
    /// Starts a new game. 
    /// </summary>
    public void StartNewGame()
    {
        TimeController.Paused = true;
        TimeController.StartNewGame();
        EmpireManager.NewGame(); 
        GalaxyGenerator.GenerateGalaxy();
        LoadMainGameScreen();
        InitCursor();
        for(int idx = 0; idx < EmpireNo; idx++)
        {
            EmpireManager.CreateNewEmpire(); 
        }
        

        TimeController.Paused = false; 
    }

    /// <summary>
    /// Loads the in game screen. 
    /// </summary>
    public void LoadMainGameScreen()
    {
        MainMenuScreen.ClearMenu();
        MainMenuScreen.AddButton("mainmenu", "esc - Main Menu", LoadMainMenu, KeyCode.Escape);
        MainMenuScreen.AddButton("pause", "space - Pause", () =>{ TimeController.Paused = !TimeController.Paused; }, KeyCode.Space);


    }
    /// <summary>
    /// Initiliazes the cursor. 
    /// </summary>
    public void InitCursor()
    {
        Cursor = PrefabManager.GetPrefab("Cursor");
        var cur = Instantiate(Cursor, new Vector3(0, 0), Cursor.transform.rotation); 
        var controller = cur.GetComponent<CursorController>();
        controller.StarSystem = this; 
    }

    /// <summary>
    /// Loads the main menu screen. 
    /// </summary>
    public void LoadMainMenu()
    {
        TimeController.EndGame(); 
        Destroy(Cursor); 
        MainMenuScreen.ClearMenu(); 
        MainMenuScreen.AddButton("newgame", "n - Start New Game", StartNewGame, KeyCode.N);
    }
}
