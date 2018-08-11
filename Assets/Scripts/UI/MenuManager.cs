using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager: MonoBehaviour {

    public MenuController MainMenuScreen;


    public TimeController TimeController; 

    public StarSystem StarSystem; 

    GameObject menuObj; 

	// Use this for initialization
	void Start () {
        StarSystem = GetComponent<StarSystem>(); 
        menuObj = GetComponent<PrefabManager>().GetPrefab("Menu");
        TimeController = GetComponent<TimeController>(); 
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
}
