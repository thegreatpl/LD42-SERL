using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleScript : ITickable {

    public StarSystem StarSystem; 

    public int CoolDown => 0;


    public List<Attributes> Entities = new List<Attributes>(); 



    public void EndTick()
    {
        
    }

    public void RunTick()
    {
        foreach(var entity in Entities)
        {
            var weapons = entity.Weapons.Where(x => x.CoolDown == 0);
            if (weapons.Count() == 0)
                continue; 


            var targets = Entities.Where(x => entity.Empire.Hostile(x.Empire));
            if (targets.Count() == 0)
                continue;
            var target = targets.Random(); 
            foreach(var w in weapons)
            {
                target.TakeDamage(w.DamageType, w.Amount);
                w.CoolDown += w.CoolDownTime; 
            }

        }
    }



    // Use this for initialization
    void Start () {
        StarSystem.TimeController.TimeObjects.Add(this);

	}
	
	// Update is called once per frame
	void Update () {
        Entities.RemoveAll(x => x == null); 
	}
}
