using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    
    /// <summary>
    /// Whether this menu is active or not. 
    /// </summary>
    public bool Active = true; 

    /// <summary>
    /// The button gameobject. 
    /// </summary>
    public GameObject ButtonObj;

    /// <summary>
    /// A text object. 
    /// </summary>
    public GameObject TextObj; 

    Dictionary<string, GameObject> buttons = new Dictionary<string, GameObject>(); 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Removes all the components from this menu. 
    /// </summary>
    public virtual void ClearMenu()
    {
        buttons = new Dictionary<string, GameObject>(); 
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject); 
        }
    }

    /// <summary>
    /// Adds a button to this. 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="onClick"></param>
    /// <param name="key"></param>
    public virtual void AddButton(string name,  string text, OnClick onClick, KeyCode? key)
    {
        try
        {
            var button = Instantiate(ButtonObj, transform);
            button.name = name;
            var control = button.GetComponent<ButtonControl>();
            control.SetValues(text, onClick, key);
            control.MenuController = this;

            buttons.Add(name, button);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error adding Button to menu:{e.Message}"); 
        }
    }

    /// <summary>
    /// Removes a specific button from the menu. 
    /// </summary>
    /// <param name="name"></param>
    public virtual void RemoveButton(string name)
    {
        if (buttons.ContainsKey(name))
        {
            Destroy(buttons[name]); 

            buttons.Remove(name);

        }
    }

    /// <summary>
    /// Adds a text reading to this object. 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="text"></param>
    public virtual void AddText(string name, string text, GetText getText = null)
    {
        var tex = Instantiate(TextObj, transform);
        tex.name = name; 
        var texto = tex.GetComponent<Text>();
        texto.text = text; 

        if (getText != null)
        {
            var update = tex.GetComponent<UpdateText>();
            update.UpdateTextDelegate = getText; 
        }

        buttons.Add(name, tex);

    }
}
