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
    /// What does the AI prioritize. 
    /// </summary>
    enum Priority
    {
        colonize,
        guard,
        attack
    }

    Priority AIPriority;

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
        //first design is random. 
        savingFor = Designs.Where(x => x.Value.Type == "Corvette" || x.Value.Type == "Colony").Random().Value;
        savingTarget = Colonies.Random();


        AIPriority = (Priority) Random.Range(0, 3); 

        foreach (var col in Colonies)
        {
            fleets.Add(new Fleet()
            {
                Location = col.Location, Tag = "guard", action = "guard"
            });
        }
        fleets.Add(new Fleet()
        {
            Location = Colonies[0].Location, Tag = "raid", action = "dock"
        });
        fleets.ForEach(x => x.HomeBase = x.Location);

        OnBuildShip += (Vector3Int location, Attributes attributes) =>
        {
            var brain = attributes.GetComponent<EntityBrain>(); 

            //set to colonize. 
            if (attributes.CanColonize)
            {
                var planets = EntityManager.GetAllUncolonized();
                if (planets.Count == 0)
                {
                    return;
                }

                IOrderedEnumerable<Vector3Int> t;
                if (Random.Range(0, 1f) < 50)
                    t =
                           from x in planets
                           orderby
                           OffsetCoord.RFromUnity(x).Distance(OffsetCoord.RFromUnity(attributes.Location)),
                           StarSystem.GetSpaceTile(x).MineralValue descending
                           select x;
                else
                    t =
                        from x in planets
                        orderby
                        StarSystem.GetSpaceTile(x).MineralValue descending,
                        OffsetCoord.RFromUnity(x).Distance(OffsetCoord.RFromUnity(attributes.Location))

                        select x;
                        

                brain.SetState(new ColonizeState(brain, t.First()));

                return;
            }
            var currentGuard = fleets.FirstOrDefault(x => x.HomeBase == location && x.Tag == "guard");
            if (currentGuard.Ships.Count < guardfleetno)
            {
                currentGuard.AddShip(brain);
                return; 
            }

            var lowGuard = from x in fleets
                           where x.Tag == "guard" && x.Ships.Count < guardfleetno
                           orderby OffsetCoord.RFromUnity(x.HomeBase).Distance(OffsetCoord.RFromUnity(attributes.Location))
                           select x; 

            if (lowGuard.Count() > 0)
            {
                var f = lowGuard.First();
                f.AddShip(brain);
                brain.SetState(new MoveState(brain, f.Location)); 
                return; 
            }

            var raid = from x in fleets
                           where x.Tag == "raid" 
                           orderby OffsetCoord.RFromUnity(x.HomeBase).Distance(OffsetCoord.RFromUnity(attributes.Location)), 
                           x.Ships.Count
                       select x;
            var r = raid.First();
            r.AddShip(brain);
            brain.SetState(new MoveState(brain, r.Location));
        };

        yield return true;

        while (true)
        {
            ShipBuilding(); 
            //if (Resouces > design.Cost)
            //{
            //    var col = Colonies.Where(x => x.ColonyAction != ColonyAction.Building).OrderBy(x => x.Colony?.MineralValue).FirstOrDefault();

            //    if (col != null)
            //    {
            //        col.BuildShip(design);
            //        design = PickDesign();
            //    }

            //}

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




            var raiders = fleets.FirstOrDefault(x => x.Tag == "raid" && x.Ships.Count == guardfleetno / 2 && x.action == "dock");
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
            var finishedRaid = fleets.Where(x => x.action == "raid" && EntityManager.GetEntitiesAt(x.Location).OfType<ColonyAttributes>().Count() == 0);
            foreach(var raid in finishedRaid)
            {
                raid.SummonFleet(raid.HomeBase); 
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

        }
    }

    ShipDesign savingFor;

    ColonyControl savingTarget;

    /// <summary>
    /// handles the shipbuilding for the ai. 
    /// </summary>
    void ShipBuilding()
    {
        if (savingFor != null)
        {
            if (savingTarget == null)
            {
                savingTarget = Colonies.Where(x => x.ColonyAction != ColonyAction.Building).Random();

            }

            if (Resouces > savingFor.Cost && savingTarget.ColonyAction != ColonyAction.Building)
            {
                savingTarget.BuildShip(savingFor);
            }
            return;
        }

        if (AIPriority == Priority.guard)
        {
            var percentage = Random.Range(0, 100);
            if (percentage < 50)
            {

                BuildGuards();
                return;

            }
            if (percentage < 90)
            {
                BuildColonies();
                return;
            }
            BuildRaiders();
        }
        if (AIPriority == Priority.attack)
        {
            var percentage = Random.Range(0, 100);
            if (percentage < 50)
            {

                BuildRaiders();
                return;

            }
            if (percentage < 75)
            {
                BuildGuards();
                return; 
            }
            BuildColonies(); 
        }




        if (AIPriority == Priority.colonize)
        {
            var percentage = Random.Range(0, 100);
            if (percentage < 50)
            {

                BuildColonies();
                return;

            }
            if (percentage < 85)
            {
                BuildGuards();
                return;
            }
            BuildRaiders();
        }
    }

    void BuildGuards()
    {
        var guardFleets = fleets.Where(x => x.Tag == "guard" && x.Ships.Count < guardfleetno).OrderBy(x => x.Ships.Count);
        if (guardFleets.Count() > 0)
        {
            BuildFleets(guardFleets);
            return;
        }
    }

    void BuildRaiders()
    {
        var guardFleets = fleets.Where(x => x.Tag == "raid").OrderBy(x => x.Ships.Count);
        if (guardFleets.Count() > 0)
        {
            BuildFleets(guardFleets);
            return;
        }
    }

    void BuildFleets(IEnumerable<Fleet> fleets)
    {
        foreach (var guard in fleets)
        {
            var colony = Colonies.FirstOrDefault(x => x.Location == guard.HomeBase);
            var design = PickDesign();
            if (Resouces < design.Cost)
            {
                savingFor = design;
                savingTarget = colony;
                return;
            }
            colony.BuildShip(design);
        }
    }

    struct Distances
    {
        public ColonyControl colony;
        public Vector3Int empty;
        public float distance; 
    }

    void BuildColonies()
    {
        List<Distances> distanceMatrix = new List<Distances>();
        var buildable = Colonies.Where(x => x.ColonyAction != ColonyAction.Building);
        var empty = EntityManager.GetAllUncolonized(); 
        foreach(var col in buildable)
        {
            foreach(var uncol in empty)
            {
                distanceMatrix.Add(new Distances()
                {
                    colony = col,
                    empty = uncol,
                    distance = OffsetCoord.RFromUnity(col.Location).Distance(OffsetCoord.RFromUnity(uncol))
                });
            }
        }
        if (distanceMatrix.Count < 1)
            return;
        var ordered = distanceMatrix.OrderBy(x => x.distance);
        var design = Designs.Where(x => x.Value.Type == "ColonyShip").Random().Value; 
        foreach(var col in ordered)
        {
            if (Resouces < design.Cost)
            {
                savingFor = design;
                savingTarget = col.colony;
                return; 
            }
            col.colony.BuildShip(design); 
        }
    }

    /// <summary>
    /// picks a random warship design. 
    /// </summary>
    /// <returns></returns>
    ShipDesign PickWarshipDesign()
    {
        var percentage = Random.Range(0, 100); 
        switch(AIPriority)
        {
            case Priority.colonize:
            case Priority.guard:
                if (percentage < 20)
                    return Designs.Where(x => x.Value.Type == "Corvette").Random().Value;
                if (percentage < 75)
                    return Designs.Where(x => x.Value.Type == "Frigate").Random().Value;
                if (percentage < 90)
                    return Designs.Where(x => x.Value.Type == "Destroyer").Random().Value;

                return Designs.Where(x => x.Value.Type == "Battleship").Random().Value;

            case Priority.attack:
                if (percentage < 10)
                    return Designs.Where(x => x.Value.Type == "Corvette").Random().Value;
                if (percentage < 75)
                    return Designs.Where(x => x.Value.Type == "Frigate").Random().Value;
                if (percentage < 85)
                    return Designs.Where(x => x.Value.Type == "Destroyer").Random().Value;

                return Designs.Where(x => x.Value.Type == "Battleship").Random().Value;

            default:
                return Designs.Where(x => x.Value.Type == "Corvette" || x.Value.Type == "Frigate" || x.Value.Type == "Destroyer" || x.Value.Type == "Battleship").Random().Value; 

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
        br.Movement = obj.GetComponent<EntityMovement>();
        br.Movement.Location = location; 
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
