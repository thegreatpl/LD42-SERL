using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager: MonoBehaviour {

    public MenuController MainMenuScreen;


    public TimeController TimeController; 

    public StarSystem StarSystem; 

    GameObject menuObj;

    GameObject PageMenuObj; 

    Dictionary<string, MenuController> Menus = new Dictionary<string, MenuController>();


    public Canvas Canvas; 

    public CursorController Cursor; 

	// Use this for initialization
	void Start () {
        StarSystem = GetComponent<StarSystem>(); 
        menuObj = GetComponent<PrefabManager>().GetPrefab("Menu");
        menuObj.GetComponentInChildren<MenuController>().ButtonObj = GetComponent<PrefabManager>().GetPrefab("Button");
        PageMenuObj = GetComponent<PrefabManager>().GetPrefab("PageMenu");
        PageMenuObj.GetComponentInChildren<PageMenu>().ButtonObj = GetComponent<PrefabManager>().GetPrefab("Button");

        TimeController = GetComponent<TimeController>();
        //Cursor = StarSystem.Cursor.GetComponent<CursorController>(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Loads the in game screen. 
    /// </summary>
    public void LoadMainGameScreen()
    {
        MainMenuScreen.ClearMenu();
        MainMenuScreen.AddButton("mainmenu", "esc - Main Menu", LoadMainMenu, KeyCode.Escape);
        MainMenuScreen.AddButton("pause", "space - Pause", () => { TimeController.Paused = !TimeController.Paused; }, KeyCode.Space);
        MainMenuScreen.AddButton("goto", "G - Goto", () => { LoadGotoMenu(); }, KeyCode.G); 
    }

    /// <summary>
    /// Loads the main menu screen. 
    /// </summary>
    public void LoadMainMenu()
    {
        TimeController.EndGame();
        Destroy(StarSystem.Cursor);
        MainMenuScreen.ClearMenu();
        MainMenuScreen.AddButton("newgame", "n - Start New Game", StarSystem.StartNewGame, KeyCode.N);
    }


    public void LoadGotoMenu()
    {
        MainMenuScreen.Active = false;
        List<ButtonDef> buttonDefs = new List<ButtonDef>();  
        var stars = StarSystem.GalaxyGenerator.Stars; 
        foreach(var star in stars)
        {
            var tile = StarSystem.GetSpaceTile(star);
            buttonDefs.Add(new ButtonDef()
            {
                name = $"goto{tile.Name}",
                text = tile.Name,
                OnClick = () => { Cursor.SetPosition(star); }
            }); 
        }
         var pageo = Instantiate(PageMenuObj, Canvas.transform);
        var page = pageo.GetComponentInChildren<PageMenu>();
        page.Close = CloseGotoMenu; 

        page.Populate(buttonDefs);
        Menus.Add("goto", page); 
    }
    public void CloseGotoMenu()
    {

        if (Menus.ContainsKey("goto"))
        {
            Destroy(Menus["goto"].transform.parent.gameObject);
            Menus.Remove("goto"); 
        }

        MainMenuScreen.Active = true; 
    }
}
