using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    public static int FlashRate = 50;
    /// <summary>
    /// Enables the flash. 
    /// </summary>
    public static bool EnableFlash = true; 

    SpriteRenderer UsualSpriteRenderer;

    SpriteRenderer EmpireSpriteRenders; 

    BaseAttributes BaseAttributes; 

    Sprite UsualTexture;

    Sprite EmpireTexture;

    int flashCounter = 0;

    bool normal = true; 

	// Use this for initialization
	void Start () {
        UsualSpriteRenderer = GetComponent<SpriteRenderer>();
        BaseAttributes = GetComponent<BaseAttributes>(); 
        UsualTexture = UsualSpriteRenderer.sprite;
        EmpireTexture = BaseAttributes.Empire.EmpireBanner;
        var go = Instantiate(new GameObject(), transform); 
        EmpireSpriteRenders = go.AddComponent<SpriteRenderer>();
        if (EmpireTexture != null)
        {
            EmpireSpriteRenders.sortingOrder = 0; 
            EmpireSpriteRenders.sprite = EmpireTexture;
            EmpireSpriteRenders.enabled = true;
        }
        else
        {
            EmpireSpriteRenders.sprite = UsualTexture;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (EnableFlash)
        {
            if (flashCounter > FlashRate)
            {
                normal = !normal;

                UsualSpriteRenderer.enabled = normal;
                // EmpireSpriteRenders.enabled = !normal;
                flashCounter = 0;
            }

            flashCounter++;
        }

	}
}
