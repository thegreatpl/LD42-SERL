using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spritemanager : MonoBehaviour {

    /// <summary>
    /// Default null sprite. 
    /// </summary>
    public Sprite NullSprite; 

    /// <summary>
    /// The loaded sprites. 
    /// </summary>
    public Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

    /// <summary>
    /// List of colors in this game. 
    /// </summary>
    public Dictionary<string, Color> Colors = new Dictionary<string, Color>(); 

	// Use this for initialization
	void Start () {
        LoadColours(); 
        LoadSprites(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadSprites()
    {
        var textures = Resources.LoadAll<Texture2D>("Tiles");

        foreach (var texture in textures)
        {
            Sprites.Add(texture.name, Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 16));
        }


    }
    /// <summary>
    /// loads the colors in. 
    /// </summary>
    public void LoadColours()
    {
       var colors = @"BLACK: 0:0:0
|BLUE: 0:0:128
|GREEN: 0:128:0
|CYAN: 0:128:128
|RED: 128:0:0
|MAGENTA: 128:0:128
|BROWN: 128:128:0
|LGREY: 192:192:192
|GREY: 128:128:128
|LBLUE: 0:0:255
|LGREEN: 0:255:0
|LCYAN: 0:255:255
|LRED: 255:0:0
|LMAGENTA: 255:0:255
|YELLOW: 255:255:0
|WHITE: 255:255:255";

        var c = colors.Split('|'); 

        foreach(var col in c)
        {
            var vals = col.Split(':');

            var color = new Color32(Convert.ToByte(vals[1].Trim()), Convert.ToByte(vals[2].Trim()), Convert.ToByte(vals[3].Trim()), 255);
            Colors.Add(vals[0], color); 
        }


    }

    /// <summary>
    /// Gets a sprite from the sprite manager. 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetSprite(string name)
    {
        if (Sprites.ContainsKey(name))
        {
            return Sprites[name]; 
        }
        return NullSprite; 
    }


}
