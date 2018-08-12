using Assets.Scripts.Empire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Entity.AI;

/// <summary>
/// When a colony is created, this gets called. 
/// </summary>
/// <param name="location"></param>
/// <param name="colonyAttributes"></param>
public delegate void OnColonize(Vector3Int location, ColonyAttributes colonyAttributes);

/// <summary>
/// When an entity is built, this gets called. 
/// </summary>
/// <param name="location"></param>
/// <param name="attributes"></param>
public delegate void OnBuildShip(Vector3Int location, Attributes attributes); 


public class EmpireScript : MonoBehaviour {

    public static int IdCount = 0; 

    public int Id;


    public string Name => $"Empire {Id}"; 

    public Sprite EmpireBanner; 

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

    /// <summary>
    /// The colony prefab. 
    /// </summary>
    public static GameObject Colony; 

    /// <summary>
    /// List of designs this empire has. 
    /// </summary>
    public Dictionary<string, ShipDesign> Designs = new Dictionary<string, ShipDesign>();

    /// <summary>
    /// List of all ships owned by this empire. 
    /// </summary>
    public List<EntityBrain> Ships = new List<EntityBrain>();

    /// <summary>
    /// The colonies of this empire. 
    /// </summary>
    public List<ColonyControl> Colonies = new List<ColonyControl>();

    List<Fleet> fleets = new List<Fleet>();

    int guardfleetno;

    int raidingFleetsize;

    /// <summary>
    /// The color this empire will be displayed as. 
    /// </summary>
    public Color EmpireColor; 


    Coroutine Ai;

    /// <summary>
    /// Delegates to be called when a colony is built. 
    /// </summary>
    public OnColonize OnColonize;

    /// <summary>
    /// Delegates to be called when a ship is built. 
    /// </summary>
    public OnBuildShip OnBuildShip; 

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

        guardfleetno = Random.Range(1, 5);
        raidingFleetsize = Random.Range(5, 20); 

        var design = Designs.Random().Value;


        foreach (var col in Colonies)
        {
            fleets.Add(new Fleet()
            {
                Location = col.Location, Tag = "guard", action = "guard"
            });
        }
        fleets.Add(new Fleet()
        {
            Location = Colonies[0].Location, Tag = "raiding", action = "dock"
        });
        fleets.ForEach(x => x.HomeBase = x.Location);
        yield return true; 

        while (true)
        {
            if (Resouces > design.Cost)
            {
                var col = Colonies.Where(x => x.ColonyAction != ColonyAction.Building).OrderBy(x => x.Colony?.MineralValue).FirstOrDefault() ;

                if (col != null)
                {
                    col.BuildShip(design);
                    design = PickDesign(); 
                }
                
            }

            yield return null; 

            var colonieships = Ships.Where(x => x.Attributes.CanColonize && x.State != "Colonize");
            if (colonieships.Count() > 0)
            {
                var planets = EntityManager.GetAllUncolonized(); 

                foreach (var cs in colonieships)
                {
                    if (planets.Count == 0)
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

            var unassigned = Ships.Where(x => x.fleet == null && x.Type == "Warship");
            foreach (var un in unassigned)
            {
                var underFleet = fleets.FirstOrDefault(x => x.Ships.Count < guardfleetno && x.Tag == "guard");
                if (underFleet == null)
                {
                    var raid = fleets.FirstOrDefault(x => x.Ships.Count < raidingFleetsize && tag == "raiding");
                    if (raid == null)
                    {
                        var nef = new Fleet() { Location = Colonies.Random().Location, Tag = "raiding", action = "dock"  };
                        nef.HomeBase = nef.Location; 
                        nef.AddShip(un);
                        un.SetState(new MoveState(un, nef.Location)); 
                        fleets.Add(nef);
                    }
                }
                else
                {
                    underFleet.AddShip(un);
                    un.SetState(new MoveState(un, underFleet.Location)); 
                }
            }

            yield return null;

            var raiders = fleets.FirstOrDefault(x => x.Ships.Count == guardfleetno / 2 && x.action == "dock"); 
            if (raiders != null)
            {
                var colonies = EntityManager.Entities.OfType<ColonyAttributes>()
                    .Where(x => x.Empire.Hostile(Id))
                    .OrderBy(x => OffsetCoord.RFromUnity(x.Location).Distance(OffsetCoord.RFromUnity(raiders.Location)));
                var target = colonies.ElementAtOrDefault(Random.Range(0, 5)); 
                if (target != null)
                {
                    raiders.SummonFleet(target.Location);
                    raiders.action = "raid"; 
                }

            }
            yield return null; 

            foreach(var colony in Colonies)
            {
                var guards = fleets.FirstOrDefault(x => x.HomeBase == colony.Location && x.Tag == "guard"); 
                if (guards == null)
                {
                    fleets.Add(new Fleet()
                    {
                        Location = colony.Location,
                        HomeBase = colony.Location,
                        action = "guard",
                        Tag = "guard"
                    }); 
                }
            }
            yield return null;

            //yield return new WaitForSeconds(60);
            //var finished = Ships.Where(x => x.State == "Guard"); 
            //foreach(var f in finished)
            //    f.MoveTo(StarSystem.GalaxyGenerator.Planets.Random());

        }
    }

    /// <summary>
    /// Picks a design. todo; make it less random. 
    /// </summary>
    /// <returns></returns>
    ShipDesign PickDesign()
    {
        var percentage = Random.Range(0, 1); 

        if (Colonies.Count < 3)
        {
            if (percentage < 0.75f)
                return Designs.Where(x => x.Value.Type == "ColonyShip").Random().Value;
            else
                return Designs.Random().Value; 
        }
        if (percentage < 0.10f)
            return Designs.Where(x => x.Value.Type == "ColonyShip").Random().Value;
        if (percentage < 0.80f)
            return Designs.Where(x => x.Value.Type == "Warship").Random().Value;
        return Designs.Random().Value;

    }

    public void CreateEntity(Vector3Int location, ShipDesign design)
    {
        var loc = StarSystem.TileToWorld(location);
        var obj =Instantiate(Entity,loc , Entity.transform.rotation);
        var atr = obj.GetComponent<Attributes>();
        atr.Id = BaseAttributes.GetId();
        atr.StarSystem = StarSystem;
        atr.Empire = this;
        atr.Location = location; 
        var br = obj.GetComponent<EntityBrain>();
        br.StarSystem = StarSystem;
        var ti = obj.GetComponent<TickControlScript>();
        ti.StarSystem = StarSystem;
        atr.TickControlScript = ti;

        EntityManager.Entities.Add(atr);
        Ships.Add(br);
        obj.name = $"{Id}:{design.Name}:{atr.Id}"; 

        atr.Initialize(design);

        OnBuildShip?.Invoke(location, atr); 

    }

    public void CreateColony(Vector3Int location, GameObject source = null)
    {
        if (EntityManager.Entities.OfType<ColonyAttributes>().FirstOrDefault(x => x.Location == location) != null)
            return; 


        var loc = StarSystem.TileToWorld(location);
        var obj = Instantiate(Colony, loc, Colony.transform.rotation);
        var atr = obj.GetComponent<ColonyAttributes>();
        atr.Id = BaseAttributes.GetId();
        atr.StarSystem = StarSystem;
        atr.Empire = this;
        atr.Location = location;
        var c = obj.GetComponent<ColonyControl>();
        c.StarSystem = StarSystem;  
        c.Location = location;
        c.Empire = this; 
        
        EntityManager.Entities.Add(atr);
        Colonies.Add(c);

        obj.name = $"{Id}:ColonyPlanet:{atr.Id}";
        OnColonize?.Invoke(location, atr); 

        if (source != null)
            Destroy(source); 
    }



}
