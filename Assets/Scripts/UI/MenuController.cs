﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    /// <summary>
    /// The button gameobject. 
    /// </summary>
    public GameObject ButtonObj;

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
    public void ClearMenu()
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
    public void AddButton(string name,  string text, OnClick onClick, KeyCode? key)
    {
        var button = Instantiate(ButtonObj, transform);
        button.name = name; 
        var control = button.GetComponent<ButtonControl>();
        control.SetValues(text, onClick, key);
        buttons.Add(name, button); 
    }

    /// <summary>
    /// Removes a specific button from the menu. 
    /// </summary>
    /// <param name="name"></param>
    public void RemoveButton(string name)
    {
        if (buttons.ContainsKey(name))
        {
            Destroy(buttons[name]); 

            buttons.Remove(name);

        }
    }
}
