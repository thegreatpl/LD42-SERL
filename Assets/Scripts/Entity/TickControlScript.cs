using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickControlScript : MonoBehaviour {



    /// <summary>
    /// How long before this entity can take another action. 
    /// </summary>
    public int CoolDown = 0; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Called during a tick. 
    /// </summary>
    public void RunTick()
    {
    }

    /// <summary>
    /// Called at the end of a tick. 
    /// </summary>
    public void EndTick()
    {
        CoolDown--;
    }
}
