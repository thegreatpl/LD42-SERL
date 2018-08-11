using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Called when a button is clicked. 
/// </summary>
public delegate void OnClick(); 

public class ButtonControl : MonoBehaviour {

    /// <summary>
    /// The attached button. 
    /// </summary>
    public Button Button;

    /// <summary>
    /// The attached text of the button. 
    /// </summary>
    public Text Text;

    OnClick OnClick;

    KeyCode? Key; 

	// Use this for initialization
	void Start () {
        Button = GetComponent<Button>();
        Text = GetComponentInChildren<Text>(); 
	}
	
	// Update is called once per frame
	void Update () {
		if (Key.HasValue && Input.GetKeyDown(Key.Value))
        {
            OnClick?.Invoke(); 
        }
	}

    /// <summary>
    /// Sets the values to be used by this button. 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="onClick"></param>
    /// <param name="key"></param>
    public void SetValues(string text, OnClick onClick, KeyCode? key)
    {
        Text.text = text;
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(new UnityEngine.Events.UnityAction(onClick));
        Key = key;
        OnClick += onClick; 
    }


}
