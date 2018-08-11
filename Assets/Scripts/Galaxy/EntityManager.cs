using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

    StarSystem StarSystem;

    GameObject BattlePrefab; 

    public List<Attributes> Entities = new List<Attributes>(); 

	// Use this for initialization
	void Start () {
        StarSystem = GetComponent<StarSystem>(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EndTick()
    {

    }
}
