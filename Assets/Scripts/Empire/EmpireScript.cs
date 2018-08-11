using Assets.Scripts.Empire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

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


    public Dictionary<string, ShipDesign> Designs = new Dictionary<string, ShipDesign>();

    public List<EntityBrain> Ships = new List<EntityBrain>(); 

    Coroutine Ai; 

	// Use this for initialization
	void Start () {
        Id = IdCount;
        IdCount++; 
	}
	
	// Update is called once per frame
	void Update () {
        Ships.RemoveAll(x => x == null); 
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
        //todo, implement diplomacy. 
        return true; 
    }


    public IEnumerator EmpireAi()
    {
        CreateEntity(StarSystem.GalaxyGenerator.Planets.Random(), Designs.Random().Value);
        CreateEntity(StarSystem.GalaxyGenerator.Planets.Random(), Designs.Random().Value);
        CreateEntity(StarSystem.GalaxyGenerator.Planets.Random(), Designs.Random().Value);
        CreateEntity(StarSystem.GalaxyGenerator.Planets.Random(), Designs.Random().Value);
        yield return null; 

        foreach(var e in Ships)
        {
            e.MoveTo(StarSystem.GalaxyGenerator.Planets.Random()); 
        }
        while (true)
        {
            yield return new WaitForSeconds(60);
            var finished = Ships.Where(x => x.State == "Guard"); 
            foreach(var f in finished)
                f.MoveTo(StarSystem.GalaxyGenerator.Planets.Random());

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



        atr.Initialize(design);

    }
}
