using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickControlScript : MonoBehaviour, ITickable {

    public StarSystem StarSystem; 

    /// <summary>
    /// How long before this entity can take another action. 
    /// </summary>
    public int CoolDown { get; set; }

    /// <summary>
    /// List of all things that use a cooldown. 
    /// </summary>
    public List<ICooldown> Cooldowns = new List<ICooldown>();

    public EntityBrain EntityBrain; 

	// Use this for initialization
	void Start () {
        CoolDown = 0;
        EntityBrain = GetComponent<EntityBrain>(); 
        StarSystem.TimeController.TimeObjects.Add(this); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Called during a tick. 
    /// </summary>
    public void RunTick()
    {
        EntityBrain.RunTick(); 
    }

    /// <summary>
    /// Called at the end of a tick. 
    /// </summary>
    public void EndTick()
    {
        CoolDown--;
        Cooldowns.ForEach(x => x.EndTick()); 
    }
}
