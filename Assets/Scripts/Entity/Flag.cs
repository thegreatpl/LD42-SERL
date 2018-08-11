using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    public static int FlashRate = 50;
    /// <summary>
    /// Enables the flash. 
    /// </summary>
    public static bool EnableFlash = true; 

    /// <summary>
    /// Empty prefab. 
    /// </summary>
    public static GameObject EmptyPrefab; 

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
        var go = Instantiate(EmptyPrefab, transform);
        go.transform.parent = transform; 
        EmpireSpriteRenders = go.AddComponent<SpriteRenderer>();
        if (EmpireTexture != null)
        {
            EmpireSpriteRenders.sortingOrder = UsualSpriteRenderer.sortingOrder-1; 
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
