using Assets.Scripts.Empire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Entity.AI;

public class EmpireScript : MonoBehaviour {

    public static int IdCount = 0; 

    public int Id;


    public int Resouces = 0;

    public StarSystem StarSystem; 


    /// <summary>
    /// Link to the entity manager. 
    /// </summary>
    public EntityManager EntityManager;

    /// <summary>
    /// The entity gameobject. 
    /// </summary>
    public static GameObject Entity;

    public static GameObject Colony; 


    public Dictionary<string, ShipDesign> Designs = new Dictionary<string, ShipDesign>();

    public List<EntityBrain> Ships = new List<EntityBrain>();

    /// <summary>
    /// The colonies of this empire. 
    /// </summary>
    public List<ColonyControl> Colonies = new List<ColonyControl>(); 

    Coroutine Ai; 

	// Use this for initialization
	void Start () {
        Id = IdCount;
        IdCount++; 
	}
	
	// Update is called once per frame
	void Update () {
        Ships.RemoveAll(x => x == null);
        Colonies.RemoveAll(x => x == null); 
	}

    public void StartAi()
    {
        Ai = StartCoroutine(EmpireAi()); 
    }

    public bool Hostile(EmpireScript empire)
    {
        return Hostile(empire.Id); 
    }

    public bool Hostile(int otherId)
    {
        if (otherId == Id)
            return false; 
        //todo, implement diplomacy. 
        return true; 
    }


    public IEnumerator EmpireAi()
    {
        yield return null;


        var design = Designs.Random().Value;

        while (true)
        {
            if (Resouces > design.Cost)
            {
                var col = Colonies.Where(x => !x.building).OrderBy(x => x.Colony?.MineralValue).FirstOrDefault() ;

                if (col != null)
                {
                    col.BuildShip(design); 
                    design = Designs.Random().Value;
                }
                
            }

            yield return null; 

            var colonieships = Ships.Where(x => x.Attributes.CanColonize && x.State != "Colonize");
            if (colonieships.Count() > 0)
            {
                var planets = EntityManager.GetAllUncolonized(); 

                foreach (var cs in colonieships)
                {
                    if (planets.Count > 0)
                    {
                        break;
                    }
                    var pos = planets.OrderBy(x => OffsetCoord.RFromUnity(x).Distance(OffsetCoord.RFromUnity(cs.Movement.Location)));
                    var t = pos.First(); 
                    cs.SetState(new ColonizeState(cs, t));
                    planets.Remove(t); 
                }
            }
            

            yield return null; 




            //yield return new WaitForSeconds(60);
            //var finished = Ships.Where(x => x.State == "Guard"); 
            //foreach(var f in finished)
            //    f.MoveTo(StarSystem.GalaxyGenerator.Planets.Random());

        }
    }

    public void CreateEntity(Vector3Int location, ShipDesign design)
    {
        var loc = StarSystem.TileToWorld(location);
        var obj =Instantiate(Entity,loc , Entity.transform.rotation);
        var atr = obj.GetComponent<Attributes>();
        atr.StarSystem = StarSystem;
        atr.Empire = this;
        atr.Location = location; 
        var br = obj.GetComponent<EntityBrain>();
        br.StarSystem = StarSystem;
        Ships.Add(br); 
        var ti = obj.GetComponent<TickControlScript>();
        ti.StarSystem = StarSystem;
        atr.TickControlScript = ti;

        EntityManager.Entities.Add(atr);
        Ships.Add(br); 

        atr.Initialize(design);

    }

    public void CreateColony(Vector3Int location, GameObject source = null)
    {
        if (EntityManager.Entities.OfType<ColonyAttributes>().FirstOrDefault(x => x.Location == location) != null)
            return; 


        var loc = StarSystem.TileToWorld(location);
        var obj = Instantiate(Colony, loc, Colony.transform.rotation);
        var atr = obj.GetComponent<ColonyAttributes>();
        atr.StarSystem = StarSystem;
        atr.Empire = this;
        atr.Location = location;
        var c = obj.GetComponent<ColonyControl>();
        c.StarSystem = StarSystem;
        c.Location = location;
        c.Empire = this; 
        
        EntityManager.Entities.Add(atr);
        Colonies.Add(c); 

        if (source != null)
            Destroy(source); 
    }



}
