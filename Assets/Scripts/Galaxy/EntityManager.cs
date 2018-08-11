using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class EntityManager : MonoBehaviour {

    StarSystem StarSystem;

    PrefabManager PrefabManager; 

    GameObject BattlePrefab; 

    public List<Attributes> Entities = new List<Attributes>();

    Dictionary<Vector3Int, BattleScript> Battles = new Dictionary<Vector3Int, BattleScript>(); 

	// Use this for initialization
	void Start () {
        StarSystem = GetComponent<StarSystem>();
        PrefabManager = GetComponent<PrefabManager>(); 
        BattlePrefab = PrefabManager.GetPrefab("Battle"); 
	}
	
	// Update is called once per frame
	void Update () {
        var dead = Battles.Where(x => x.Value == null);
        foreach (var d in dead)
            Battles.Remove(d.Key); 
	}

    public void EndTick()
    {
        foreach (var ent in Entities)
        {
            if (ent.Battle != null)
                continue;
            var otherHostiles = Entities.Where(x => x.Location == ent.Location && ent.Empire.Hostile(x.Empire));
            if (otherHostiles.Count() > 0)
            {
                if (Battles.ContainsKey(ent.Location))
                {
                    Battles[ent.Location].Entities.Add(ent); 
                }
                else
                {
                   var bat = Instantiate(BattlePrefab, StarSystem.TileToWorld(ent.Location), BattlePrefab.transform.rotation).GetComponent<BattleScript>();
                    bat.StarSystem = StarSystem; 
                    Battles.Add(ent.Location, bat);
                    bat.Entities.Add(ent); 

                }
            }
        }
    }
}
