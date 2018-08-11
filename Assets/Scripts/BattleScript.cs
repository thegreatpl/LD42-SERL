using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleScript : MonoBehaviour, ITickable {

    public StarSystem StarSystem; 

    public int CoolDown => 0;


    public List<BaseAttributes> Entities = new List<BaseAttributes>();

    public SpriteRenderer SpriteRenderer; 


    public void EndTick()
    {
        
    }

    public void RunTick()
    {
        foreach(var entity in Entities.OfType<Attributes>())
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
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.sprite = StarSystem.Spritemanager.GetSprite("battle"); 

	}
	
	// Update is called once per frame
	void Update () {
        Entities.RemoveAll(x => x == null); 
	}
}
