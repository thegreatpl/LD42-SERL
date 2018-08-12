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

    public LogScript Logger; 

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
        Logger = StarSystem.Logger; 
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
        MainMenuScreen.AddButton("colonies", "H - Colonies", () => { LoadSelectObjectPageMenu(Cursor.PlayerEmpire.Colonies.Select(x => x.GetComponent<BaseAttributes>())); }, KeyCode.H);
        MainMenuScreen.AddButton("ships", "J - Ships", () => { LoadSelectObjectPageMenu(Cursor.PlayerEmpire.Ships.Select(x => x.GetComponent<BaseAttributes>())); }, KeyCode.J);
        MainMenuScreen.AddButton("ping", "P - Goto Last Message", () => { Cursor.SetPosition(Logger.PingLocation); }, KeyCode.P);
        MainMenuScreen.AddButton("look", "L - Look", () =>
        {
            var loc = Cursor.Movement.Location; 
            if (StarSystem.EntityManager.Battles.ContainsKey(loc))
            {
                LoadBattleMenu(StarSystem.EntityManager.Battles[loc]);
                return; 
            }
            LoadLookMenu(loc); 
        }, KeyCode.L); 
        MainMenuScreen.AddButton("flag", "I - Toggle Flags", () => { Flag.EnableFlash = !Flag.EnableFlash; }, KeyCode.I);
        Cursor.SetMovement(true, MainMenuScreen); 
    }

    /// <summary>
    /// Loads the main menu screen. 
    /// </summary>
    public void LoadMainMenu()
    {
        TimeController?.EndGame();
        StarSystem?.EmpireManager?.EndGame(); 
        if (Cursor != null)
            Destroy(Cursor.gameObject);
        MainMenuScreen.ClearMenu();
        MainMenuScreen.AddButton("newgame", "n - Start New Game", StarSystem.StartNewGame, KeyCode.N);
    }

    /// <summary>
    /// Loads the goto menu. 
    /// </summary>
    public void LoadGotoMenu()
    {
        MainMenuScreen.Active = false;
        List<PageObjectDef> buttonDefs = new List<PageObjectDef>();  
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

        var entities = StarSystem.EntityManager.GetEntitiesAt(location).Where(x => x.Empire == Cursor.PlayerEmpire);
        LoadSelectObjectPageMenu(entities); 
    }

    public void LoadSelectObjectPageMenu(IEnumerable<BaseAttributes> entities)
    {
        CloseMenu("selectpage"); 
        if (entities.Count() == 0)
            return;
        if (entities.Count() == 1)
        {
            LoadSelectedMenu(entities.ElementAt(0));
            return; 
        }

        SetAllInactive();
        List<PageObjectDef> buttonDefs = new List<PageObjectDef>();
        foreach(var ent in entities)
        {
            buttonDefs.Add(new ButtonDef()
            {
                name = $"select{ent.Id}",
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
        CloseMenu("select"); 
        var pageo = Instantiate(menuObj, Canvas.transform);
        var menu= pageo.GetComponentInChildren<MenuController>();
        menu.AddText("selectobj", attributes.name); 
        var colony = attributes as ColonyAttributes;
        var entity = attributes as Attributes;
        if (colony != null)
        {
            var colonyControl = colony.GetComponent<ColonyControl>();
            menu.AddText("status", "", () => { return colonyControl.ColonyAction.ToString(); }); 
        }
        else if (entity != null)
        {
            var brain = entity.GetComponent<EntityBrain>();
            menu.AddText("status", "", () => { return brain.Action; }); 
        }



        Cursor.SetMovement(true, menu);
        menu.AddButton("deselect", "Backspace - Back", () => { CloseMenu("select"); }, KeyCode.Backspace);
        menu.AddButton("goto", "G - Goto", () => { Cursor.SetPosition(attributes.Location); }, KeyCode.G); 
        if (colony != null)
        {
            var colonyControl = colony.GetComponent<ColonyControl>();

            menu.AddButton("build", "B - Build", () =>
            {
                if (colonyControl == null)
                { CloseMenu("select"); return; }
                if (colonyControl.ColonyAction == ColonyAction.Building)
                {
                    Logger.Log("Colony is building!", colonyControl.Location); 
                    return;
                }

                LoadBuildMenu(colonyControl);      
            }, KeyCode.B); 

            SetAllInactive(); 
            Menus.Add("select", menu); 
            return; 
        }
        if (entity != null)
        {
            var brain = entity.GetComponent<EntityBrain>();
            menu.AddButton("moveentity", "M - MoveToCursor", () => {
                if (brain == null)
                {
                    Logger.Log("Ship is Dead!", Cursor.Movement.Location); 
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
                        Logger.Log("Ship is Dead!", Cursor.Movement.Location);
                        CloseMenu("select");
                        return;
                    }
                    if (StarSystem.EntityManager.GetAllUncolonized().Contains(Cursor.Movement.Location))
                    {
                        brain.SetState(new ColonizeState(brain, Cursor.Movement.Location));
                        CloseMenu("select");
                        return;
                    }
                    Logger.Log("Not Valid for colonization", Cursor.Movement.Location);
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
        CloseMenu("build"); 
        SetAllInactive(); 
        List<PageObjectDef> buttonDefs = new List<PageObjectDef>();

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
                    if (colonyControl.ColonyAction != ColonyAction.Building )
                    {
                        if (colonyControl.Empire.Resouces > design.Value.Cost)
                        {
                            colonyControl.BuildShip(design.Value);
                            CloseMenu("build");
                            return; 
                        }
                        Logger.Log("Not enough resources", colonyControl.Location);
                        return;
                    }
                    Logger.Log("Colony is building currently", colonyControl.Location);


                }, text = $"{design.Key}: {design.Value.Type}{Environment.NewLine}"
                + $"{design.Value.Cost} L:{weapons.Where(x => x.DamageType == DamageType.Energy).Sum(x => x.Amount)} M:{weapons.Where(x => x.DamageType == DamageType.Mass).Sum(x => x.Amount)} O:{weapons.Where(x => x.DamageType == DamageType.Mass).Sum(x => x.Amount)}", 
            });
        }

        page.Close = () => { CloseMenu("build"); };

        page.Populate(buttonDefs);
        Menus.Add("build", page); 

    }

    /// <summary>
    /// Loads up a battle for the player to view. 
    /// </summary>
    /// <param name="battle"></param>
    public void LoadBattleMenu(BattleScript battle)
    {
        CloseMenu("battle");
        SetAllInactive(); 
        var pageo = Instantiate(PageMenuObj, Canvas.transform);
        var page = pageo.GetComponentInChildren<PageMenu>();

        var text = new List<PageObjectDef>();
        text.Add(new PageObjectDef()
        {
            name = "battle",
            text = $"Battle At {battle.Location}"
        }); 
        foreach(var entity in battle.Entities)
        {
            text.Add(new UpdateTextDef()
            {
                name = $"battle{entity.Id}",
                text = "",
                UpdateText = () =>
                    {
                        if (entity == null)
                            return "Wreckage";
                        if (entity.Battle != battle)
                            return $"{entity.name}: Fled the field"; 

                        return $"{entity.name} : {entity.Type} {Environment.NewLine}" +
                        $"H:{entity.HP}/{entity.MaxHP} A:{entity.Armor}/{entity.MaxArmor} S:{entity.Shields}/{entity.MaxShields}{Environment.NewLine}"; 
                    }
            }); 
        }
        page.Close = () => { CloseMenu("battle"); }; 
        page.Populate(text);
        Menus.Add("battle", page);

    }

    public void LoadLookMenu(Vector3Int location)
    {
        var entities = StarSystem.EntityManager.GetEntitiesAt(location); 
        if (entities.Count() < 1)
        {

            if (StarSystem.GalaxyGenerator.Stars.Contains(location))
            {
               var tile = StarSystem.GetSpaceTile(location);
                Logger.Log($"The star {tile.Name}", location);
                return; 
            }
            if (StarSystem.GalaxyGenerator.Planets.Contains(location))
            {
                var tile = StarSystem.GetSpaceTile(location);
                Logger.Log($"The planet {tile.Name}. Resources of {tile.MineralValue}", location);
                return;
            }

            Logger.Log("There is nothing here", location); 
            return; 
        }

        CloseMenu("look");
        SetAllInactive();
        var pageo = Instantiate(PageMenuObj, Canvas.transform);
        var page = pageo.GetComponentInChildren<PageMenu>();
        List<PageObjectDef> pageObjectDefs = new List<PageObjectDef>(); 
        foreach(var entity in entities)
        {
            pageObjectDefs.Add(new UpdateTextDef()
            {
                name = $"look{entity.name}", text = "", UpdateText = () =>
                {
                    if (entity == null)
                        return "Wreckage";
                    return $"{entity.name} : {entity.Type} {Environment.NewLine}" +
                       $"H:{entity.HP}/{entity.MaxHP} A:{entity.Armor}/{entity.MaxArmor} S:{entity.Shields}/{entity.MaxShields}{Environment.NewLine}";

                }
            }); 
        }

        
        page.Close = () => { CloseMenu("look"); };
        page.Populate(pageObjectDefs); 
        Menus.Add("look", page);


    }



}
