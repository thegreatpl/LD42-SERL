using Assets.Scripts.Entity.AI;
using Assets.Scripts.Entity.Components;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        menuObj.GetComponentInChildren<MenuController>().TextObj = GetComponent<PrefabManager>().GetPrefab("Text");

        PageMenuObj = GetComponent<PrefabManager>().GetPrefab("PageMenu");
        PageMenuObj.GetComponentInChildren<PageMenu>().ButtonObj = GetComponent<PrefabManager>().GetPrefab("Button");
        PageMenuObj.GetComponentInChildren<MenuController>().TextObj = GetComponent<PrefabManager>().GetPrefab("Text");

        TimeController = GetComponent<TimeController>();
        //Cursor = StarSystem.Cursor.GetComponent<CursorController>(); 
	}
	
	// Update is called once per frame
	void Update () {
        if (!MainMenuScreen.Active && (Menus.Count < 1 || Menus.Where(x => x.Value.Active).Count() < 1))
        {
            MainMenuScreen.Active = true;
            for (int idx = 0; Menus.Count > idx; )
            {
               CloseMenu(Menus.ElementAt(0).Key);
            }
        }
        
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
        MainMenuScreen.AddButton("select", "S - Select", LoadSelectObjectPageMenu, KeyCode.S); 
        MainMenuScreen.AddButton("flag", "P - Toggle Flags", () => { Flag.EnableFlash = !Flag.EnableFlash; }, KeyCode.P);
        Cursor.SetMovement(true, MainMenuScreen); 
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

    /// <summary>
    /// Loads the goto menu. 
    /// </summary>
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

    void SetAllInactive()
    {
        MainMenuScreen.Active = false;
        foreach (var m in Menus)
        {
            m.Value.Active = false;
        }
    }


    public void LoadSelectObjectPageMenu()
    {
        var location = Cursor.Movement.Location;

        var entities = StarSystem.EntityManager.GetEntitiesAt(location);

        if (entities.Count() == 0)
            return;
        if (entities.Count() == 1)
        {
            LoadSelectedMenu(entities.ElementAt(0));
            return; 
        }

        SetAllInactive();
        List<ButtonDef> buttonDefs = new List<ButtonDef>();
        foreach(var ent in entities)
        {
            buttonDefs.Add(new ButtonDef()
            {
                name = $"select{ent.name}",
                OnClick = () => { LoadSelectedMenu(ent); CloseMenu("selectpage"); },
                text = $"{ent.Type}:{ent.Location.ToString()}"
            });
        }

        var pageo = Instantiate(PageMenuObj, Canvas.transform);
        var page = pageo.GetComponentInChildren<PageMenu>();
        page.Close = () => { CloseMenu("selectpage"); };

        page.Populate(buttonDefs);
        Menus.Add("selectpage", page);

    }
    /// <summary>
    /// Closes the menu with the given name. 
    /// </summary>
    /// <param name="name"></param>
    public void CloseMenu(string name)
    {
        if (Menus.ContainsKey(name))
        {
            Destroy(Menus[name].transform.parent.gameObject);
            Menus.Remove(name); 
        }

        if (Menus.Count < 1)
            MainMenuScreen.Active = true; 
    }

    /// <summary>
    /// Loads the selected menu. 
    /// </summary>
    /// <param name="attributes"></param>
    public void LoadSelectedMenu(BaseAttributes attributes)
    {
        var pageo = Instantiate(menuObj, Canvas.transform);
        var menu= pageo.GetComponentInChildren<MenuController>();
        Cursor.SetMovement(true, menu);
        menu.AddButton("deselect", "Backspace - Back", () => { CloseMenu("select"); }, KeyCode.Backspace);
        menu.AddButton("goto", "G - Goto", () => { Cursor.SetPosition(attributes.Location); }, KeyCode.G); 
        var colony = attributes as ColonyAttributes; 
        if (colony != null)
        {
            var colonyControl = colony.GetComponent<ColonyControl>();

            menu.AddButton("build", "B - Build", () =>
            {
                if (colonyControl == null)
                { CloseMenu("select"); return; }
                if (colonyControl.building)
                    return; 

                LoadBuildMenu(colonyControl);      
            }, KeyCode.B); 

            SetAllInactive(); 
            Menus.Add("select", menu); 
            return; 
        }
        var entity = attributes as Attributes; 
        if (entity != null)
        {
            var brain = entity.GetComponent<EntityBrain>();
            menu.AddButton("moveentity", "M - MoveToCursor", () => {
                if (brain == null)
                {
                    CloseMenu("select"); return;
                }
                brain.SetState(new MoveState(brain, Cursor.Movement.Location));
                CloseMenu("select");
                return;
            }, KeyCode.M);

            if (entity.CanColonize)
                menu.AddButton("colonize", "O - Colonize", () =>
                {
                    if (brain == null)
                    {
                        CloseMenu("select");
                        return;
                    }
                    if (StarSystem.EntityManager.GetAllUncolonized().Contains(Cursor.Movement.Location))
                    {
                        brain.SetState(new ColonizeState(brain, Cursor.Movement.Location));
                        CloseMenu("select");
                        return;
                    }
                }, KeyCode.O); 


            SetAllInactive();
            Menus.Add("select", menu);
            return; 
        }


    }


    /// <summary>
    /// Loads up the build menu for the given colony. 
    /// </summary>
    /// <param name="colonyControl"></param>
    public void LoadBuildMenu(ColonyControl colonyControl)
    {
        SetAllInactive(); 
        List<ButtonDef> buttonDefs = new List<ButtonDef>();

        var pageo = Instantiate(PageMenuObj, Canvas.transform);
        var page = pageo.GetComponentInChildren<PageMenu>();
        foreach(var design in Cursor.PlayerEmpire.Designs)
        {
            var weapons = design.Value.BaseComponents.OfType<WeaponComponent>(); 
            buttonDefs.Add(new ButtonDef()
            {
                name = $"design{design.Key}",
                OnClick = () =>
                {
                    CloseMenu("build");
                    if (colonyControl.building == false)
                    {
                        if (colonyControl.Empire.Resouces > design.Value.Cost)
                            colonyControl.BuildShip(design.Value);
                    }
                }, text = $"{design.Key}: {design.Value.Type}{Environment.NewLine}"
                + $"{design.Value.Cost} L:{weapons.Where(x => x.DamageType == DamageType.Energy).Sum(x => x.Amount)} M:{weapons.Where(x => x.DamageType == DamageType.Mass).Sum(x => x.Amount)} O:{weapons.Where(x => x.DamageType == DamageType.Mass).Sum(x => x.Amount)}", 
            });
        }

        page.Close = () => { CloseMenu("build"); };

        page.Populate(buttonDefs);
        Menus.Add("build", page); 

    }
}
