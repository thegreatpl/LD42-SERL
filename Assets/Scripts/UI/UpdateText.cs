using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Function to call to get an updated string value. 
/// </summary>
/// <returns></returns>
public delegate string GetText(); 


[RequireComponent(typeof(Text))]
public class UpdateText : MonoBehaviour {

    public GetText UpdateTextDelegate;


    public Text Text; 
	// Use this for initialization
	void Start () {
        Text = GetComponent<Text>(); 
	}
	
	// Update is called once per frame
	void Update () {
		if (UpdateTextDelegate != null)
        {
            Text.text = UpdateTextDelegate(); 
        }
	}
}
