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

    public Dictionary<int, List<PageObjectDef>> Pages = new Dictionary<int, List<PageObjectDef>>();

    public OnClick Close; 

    /// <summary>
    /// Populates the page menu. 
    /// </summary>
    /// <param name="definitions"></param>
    public void Populate(List<PageObjectDef> definitions)
    {
        Pages = new Dictionary<int, List<PageObjectDef>>(); 
        List<PageObjectDef> currentPage = new List<PageObjectDef>();
        Pages.Add(Pages.Count, currentPage);
        foreach (var def in definitions)
        {
            if (currentPage.Count >= PerPage)
            {
                currentPage = new List<PageObjectDef>();
                Pages.Add(Pages.Count, currentPage); 
            }
            currentPage.Add(def); 
        }

        PopulatePage(0); 
    }

    /// <summary>
    /// Populates the given page with data. 
    /// </summary>
    /// <param name="id"></param>
    public void PopulatePage(int id)
    {
        ClearMenu();
        int idx = -1; 
        foreach(var option in Pages[id])
        {
            idx++; 

            var button = option as ButtonDef;
            if (button != null)
            {
                AddButton(option.name, $"{idx} - {button.text}", button.OnClick, (KeyCode)(48 + idx));
                continue; 
            }

            var text = option as UpdateTextDef; 
            if (text != null)
            {
                AddText(text.name, text.text, text.UpdateText); 
                continue; 
            }

            AddText(option.name, option.text); 
                   

        }


        if (id != 0)
            AddButton("left", $"A - Previous {PerPage}", () => { PopulatePage(id - 1); }, KeyCode.A);
        if (id != Pages.Count - 1)
            AddButton("right", $"D - Next {PerPage}", () => { PopulatePage(id + 1); }, KeyCode.D);

        AddButton("game", $"Backspace - Game", Close, KeyCode.Backspace);
    }
}

