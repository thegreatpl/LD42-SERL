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

    public Text Resource; 

    public Camera Camera;

    public GalaxyGenerator GalaxyGenerator;

    public Spritemanager Spritemanager;

    public PrefabManager PrefabManager;

    public TimeController TimeController;

    public EntityManager EntityManager;

    public EmpireManager EmpireManager;

    public MenuManager MenuManager; 

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
        MenuManager = GetComponent<MenuManager>(); 
        Flag.EmptyPrefab = new GameObject(); 
        MenuManager.LoadMainMenu(); 
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

        //start player empire. 
        StartPlayerEmpire(); 
        for(int idx = 0; idx < EmpireNo; idx++)
        {
            var emp = EmpireManager.CreateNewEmpire();
            if (emp == null)
                continue; 
            emp.StartAi(); 
        }
        
        MenuManager.LoadMainGameScreen();

        TimeController.Paused = false; 
    }

    void StartPlayerEmpire()
    {

        var player = EmpireManager.CreateNewEmpire();
        player.EmpireColor = Spritemanager.Colors["LGREEN"]; 

        InitCursor(TileToWorld(player.Colonies[0].Location));

        MenuManager.Cursor.PlayerEmpire = player;
 
        var curmov = Cursor.GetComponent<Movement>();
        curmov.StarSystem = this;
        curmov.Move(player.Colonies[0].Location);
        MenuManager.Cursor.Movement = curmov; 
        MenuManager.Cursor.SetCamPos(); 
    }

   
    /// <summary>
    /// Initiliazes the cursor. 
    /// </summary>
    public void InitCursor(Vector3 location)
    {
        Cursor = PrefabManager.GetPrefab("Cursor");
        var cur = Instantiate(Cursor, location, Cursor.transform.rotation); 
        var controller = cur.GetComponent<CursorController>();
        controller.StarSystem = this;
        controller.Camera = Camera; 
        MenuManager.Cursor = controller; 
    }

   
}
