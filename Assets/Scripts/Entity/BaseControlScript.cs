using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseControlScript : MonoBehaviour {

    public int Cooldown = 0; 

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RunTick()
    {
        DoAction(); 
    }

    protected abstract void DoAction(); 

    public void EndTick()
    {
        Cooldown--; 
    }

}
