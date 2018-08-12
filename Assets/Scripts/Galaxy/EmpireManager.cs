using Assets.Scripts.Empire;
using Assets.Scripts.Entity.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmpireManager : MonoBehaviour {

    public StarSystem StarSystem;

    public Spritemanager Spritemanager; 

    public List<EmpireScript> Empires = new List<EmpireScript>();

    public EntityManager EntityManager;

    public Dictionary<string, ShipDesign> BasicDesigns = new Dictionary<string, ShipDesign>();

    public List<BaseComponent> components = new List<BaseComponent>();


    public List<Sprite> EmpireSprites = new List<Sprite>(); 

	// Use this for initialization
	void Start () {
        StarSystem = GetComponent<StarSystem>();
        EntityManager = GetComponent<EntityManager>();
        Spritemanager = GetComponent<Spritemanager>();
        NewGame(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public EmpireScript CreateNewEmpire()
    {
        if (EmpireScript.Entity == null)
            EmpireScript.Entity = StarSystem.PrefabManager.GetPrefab("Entity");
        if (EmpireScript.Colony == null)
            EmpireScript.Colony = StarSystem.PrefabManager.GetPrefab("Colony"); 

        var prefab = StarSystem.PrefabManager.GetPrefab("Empire");
        var script = Instantiate(prefab).GetComponent<EmpireScript>();
        Empires.Add(script);

        var uncolonized = EntityManager.GetAllUncolonized();
        if (uncolonized.Count < 1)
            return null;
        if (EmpireSprites.Count > 1)
        {
            var banner = EmpireSprites.Random();
            EmpireSprites.Remove(banner);
            script.EmpireBanner = banner;
        }
        script.EntityManager = EntityManager;
        script.Designs = BasicDesigns;
        script.StarSystem = StarSystem;
        script.Resouces = 100; 

        script.CreateColony(uncolonized.Random());

        script.EmpireColor = Spritemanager.Colors["RED"]; 
        //script.StartAi(); 

        return script; 
    }

    public BaseComponent GetComponentType(string type)
    {
        return components.FirstOrDefault(x => x.Types.Contains(type)); 

    }

    public void LoadComponents()
    {
        components = new List<BaseComponent>()
        {
             new WeaponComponent()
                {
                    Amount = 1,
                 CoolDownTime = 3,
                 DamageType = DamageType.Mass,
                 Name = "Cannon",
                 Weight = 0.5f,
                 Cost = 5,
                 Types = new List<string>(){"weapon", "mass"}
                },
                new EngineComponent()
                {
                    Power = 1, Weight = 0.5f, Name = "Engine", Cost = 20, Types = new List<string>(){"engine"}
                },
                 new ColonyComponent()
               {
                     Name = "Colony",
                     Types = new List<string>() { "colony"},
                   Cost = 100,
                   Weight = 50
               },
                 new WeaponComponent()
                 {
                     Amount = 1,
                     CoolDownTime = 3,
                     DamageType = DamageType.Energy,
                     Name = "Laser",
                     Weight = 0.5f,
                     Cost = 5,
                     Types = new List<string>(){"weapon", "laser"}
                 },
                 new ArmorComponent()
                 {
                     ArmorValue = 10,
                     Cost = 15,
                     Name = "Armor",
                     Types = new List<string>(){"armor"},
                     Weight = 10
                 },
                 new ShieldComponent()
                 {
                     ShieldValue = 10,
                     Cost = 15,
                     Name = "Shield",
                     Weight = 5,
                     Types = new List<string>(){"shield"}
                 },
                 new WeaponComponent()
                 {
                     Amount = 20,
                     CoolDownTime = 20,
                     Name = "Missile",
                     Cost = 20,
                     DamageType = DamageType.None,
                     Weight = 1,
                     Types = new List<string>(){"weapon", "missle"}
                 }
        };

    }

    public void LoadBasicDesigns()
    {
        BasicDesigns = new Dictionary<string, ShipDesign>();
        var corvette = new ShipDesign()
        {
            Name = "Corvette",
            Type = "Warship",
            Sprite = Spritemanager.GetSprite("corvette"),
            MaxHp = 10,
            BaseComponents = new List<BaseComponent>()
            {
                GetComponentType("engine"), GetComponentType("weapon")
            }
        };
        corvette.Cost = corvette.BaseComponents.Sum(x => x.Cost); 
        BasicDesigns.Add(corvette.Name, corvette);
        var colony = new ShipDesign()
        {
            Name = "Colony",
            Type = "ColonyShip",
            Sprite = Spritemanager.GetSprite("colonyship"),
            MaxHp = 10,
            BaseComponents = new List<BaseComponent>()
            {
                GetComponentType("engine"),GetComponentType("engine"),GetComponentType("engine"), GetComponentType("colony")
            }
        };
        colony.Cost = colony.BaseComponents.Sum(x => x.Cost); 
        BasicDesigns.Add(colony.Name, colony);
    }

    public void LoadEmpireBanners()
    {
        EmpireSprites = new List<Sprite>(); 
        int idx = 1; 
        string name = $"empire{idx}"; 
        while(Spritemanager.HasSprite(name))
        {
            EmpireSprites.Add(Spritemanager.GetSprite(name)); 
            idx++;
            name = $"empire{idx}";
        }
    }

    public void NewGame()
    {
        foreach (var empire in Empires)
        {
            empire.Ships.ForEach(x => Destroy(x));
            empire.Colonies.ForEach(x => Destroy(x)); 
            Destroy(empire); 
        }
        Empires.Clear();
        LoadEmpireBanners(); 
        LoadComponents();
        LoadBasicDesigns();
        
    }

}
