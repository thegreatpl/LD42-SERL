using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyControl : MonoBehaviour, ITickable {

    public EmpireScript Empire;

    public int CoolDown => coolDown;


    public int coolDown; 

    public void EndTick()
    {
        coolDown--;
        if (coolDown < 0)
            coolDown = 0; 
    }

    public void RunTick()
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
