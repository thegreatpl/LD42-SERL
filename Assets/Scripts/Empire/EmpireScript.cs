using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpireScript : MonoBehaviour {

    public static int IdCount = 0; 

    public int Id;

    /// <summary>
    /// Link to the entity manager. 
    /// </summary>
    public EntityManager EntityManager; 

	// Use this for initialization
	void Start () {
        Id = IdCount;
        IdCount++; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool Hostile(EmpireScript empire)
    {
        return Hostile(empire.Id); 
    }

    public bool Hostile(int otherId)
    {
        //todo, implement diplomacy. 
        return true; 
    }
}
