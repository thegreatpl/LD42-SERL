using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spritemanager : MonoBehaviour {

    public Sprite NullSprite; 

    public Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>(); 

	// Use this for initialization
	void Start () {
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
