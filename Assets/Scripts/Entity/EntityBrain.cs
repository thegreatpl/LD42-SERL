using Assets.Scripts.Empire;
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

    public string State { get { return CurrentState.Type; } }

    /// <summary>
    /// human readble string of what the entity is currently up to. 
    /// </summary>
    public string Action {
        get
        {
            if (Attributes.Battle != null)
                return "In Combat";

            return State; 
        }
    }
    /// <summary>
    /// The type of ship this is. 
    /// </summary>
    public string Type { get { return Attributes.Type; } } 

    public Fleet fleet; 

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
        CurrentState?.OnChange();
        CurrentState = newState;
        CurrentState.OnSet(); 
    }


    public void MoveTo(Vector3Int location)
    {
        SetState(new MoveState(this, location)); 
    }
}
