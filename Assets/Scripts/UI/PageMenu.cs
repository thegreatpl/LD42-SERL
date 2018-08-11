using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PageMenu : MenuController
{

    public MenuManager MenuManager; 

    public int PerPage = 10; 

    public Dictionary<int, List<ButtonDef>> Pages = new Dictionary<int, List<ButtonDef>>();

    public OnClick Close; 

    /// <summary>
    /// Populates the page menu. 
    /// </summary>
    /// <param name="definitions"></param>
    public void Populate(List<ButtonDef> definitions)
    {
        Pages = new Dictionary<int, List<ButtonDef>>(); 
        List<ButtonDef> currentPage = new List<ButtonDef>();
        Pages.Add(Pages.Count, currentPage);
        foreach (var def in definitions)
        {
            if (currentPage.Count >= PerPage)
            {
                currentPage = new List<ButtonDef>();
                Pages.Add(Pages.Count, currentPage); 
            }
            currentPage.Add(def); 
        }

        PopulatePage(1); 
    }

    /// <summary>
    /// Populates the given page with data. 
    /// </summary>
    /// <param name="id"></param>
    public void PopulatePage(int id)
    {
        ClearMenu();
        int idx = 0; 
        foreach(var option in Pages[id])
        {
            AddButton(option.name, $"{idx} - {option.text}", option.OnClick, (KeyCode)(48 + idx));
            idx++; 
        }


        if (id != 0)
            AddButton("left", $"A - Previous {PerPage}", () => { PopulatePage(id - 1); }, KeyCode.A);
        if (id != Pages.Count - 1)
            AddButton("right", $"D - Next {PerPage}", () => { PopulatePage(id + 1); }, KeyCode.D);

        AddButton("game", $"Backspace - Game", Close, KeyCode.Backspace);
    }
}

