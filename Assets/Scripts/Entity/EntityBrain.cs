using Assets.Scripts.Entity.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBrain : MonoBehaviour {

    public StarSystem StarSystem; 

    public Attributes Attributes;

    public EntityMovement Movement;

    public EntityManager EntityManager; 

    BaseState CurrentState; 

	// Use this for initialization
	void Start () {
        Attributes = GetComponent<Attributes>();
        Movement = GetComponent<EntityMovement>();

        EntityManager = StarSystem.EntityManager;

        CurrentState = new GuardState(this); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RunTick()
    {
        CurrentState.Run(); 
    }

    public void SetState(BaseState newState)
    {
        CurrentState.OnChange();
        CurrentState = newState;
        CurrentState.OnSet(); 
    }
}
