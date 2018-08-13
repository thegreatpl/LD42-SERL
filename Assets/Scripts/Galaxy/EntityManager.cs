using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class EntityManager : MonoBehaviour {

    StarSystem StarSystem;

    PrefabManager PrefabManager; 

    GameObject BattlePrefab; 

    public List<BaseAttributes> Entities = new List<BaseAttributes>();

    public Dictionary<Vector3Int, BattleScript> Battles = new Dictionary<Vector3Int, BattleScript>(); 

	// Use this for initialization
	void Start () {
        StarSystem = GetComponent<StarSystem>();
        PrefabManager = GetComponent<PrefabManager>(); 
        BattlePrefab = PrefabManager.GetPrefab("Battle"); 
	}
	
	// Update is called once per frame
	void Update () {
        Entities.RemoveAll(x => x == null); 
        var dead = Battles.Where(x => x.Value == null);
        for (int idx = 0; idx < dead.Count(); idx++)
            Battles.Remove(dead.ElementAt(idx).Key); 
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
                    ent.Battle = Battles[ent.Location]; 
                }
                else
                {
                   var bat = Instantiate(BattlePrefab, StarSystem.TileToWorld(ent.Location), BattlePrefab.transform.rotation).GetComponent<BattleScript>();
                    bat.StarSystem = StarSystem;
                    bat.Location = ent.Location; 
                    Battles.Add(ent.Location, bat);
                    bat.Entities.Add(ent);
                    ent.Battle = bat; 

                }
            }
        }


    }

    /// <summary>
    /// Gets all uncolonized systems. 
    /// </summary>
    /// <returns></returns>
    public List<Vector3Int> GetAllUncolonized()
    {
        var planets = StarSystem.GalaxyGenerator.Planets;
        var colonies = Entities.OfType<ColonyAttributes>().Select(x => x?.Location);

        planets.RemoveAll(x => colonies.Contains(x));
        return planets; 
    }

    public IEnumerable<BaseAttributes> GetEntitiesAt(Vector3Int loc)
    {
        return Entities.Where(x => x?.Location == loc); 
    }

  
}
