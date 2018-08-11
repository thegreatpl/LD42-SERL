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

	// Use this for initialization
	void Start () {
        StarSystem = GetComponent<StarSystem>();
        EntityManager = GetComponent<EntityManager>();
        Spritemanager = GetComponent<Spritemanager>(); 

        LoadBasicDesigns(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateNewEmpire()
    {
        if (EmpireScript.Entity == null)
            EmpireScript.Entity = StarSystem.PrefabManager.GetPrefab("Entity"); 

        var prefab = StarSystem.PrefabManager.GetPrefab("Empire");
        var script = Instantiate(prefab).GetComponent<EmpireScript>();
        Empires.Add(script);

        script.EntityManager = EntityManager;
        script.Designs = BasicDesigns;
        script.StarSystem = StarSystem;
        script.StartAi(); 
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
                   Weight = 100
               },
        };

    }

    public void LoadBasicDesigns()
    {
        BasicDesigns = new Dictionary<string, ShipDesign>();
        var corvette = new ShipDesign()
        {
            Name = "Corvette",
            Type = "Corvette",
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
            Type = "Colony",
            Sprite = Spritemanager.GetSprite("colony"),
            MaxHp = 10,
            BaseComponents = new List<BaseComponent>()
            {
                GetComponentType("engine"), GetComponentType("colony")
            }
        };
        colony.Cost = colony.BaseComponents.Sum(x => x.Cost); 
        BasicDesigns.Add(colony.Name, colony);
    }


    public void NewGame()
    {
        Empires.ForEach(x => Destroy(x.gameObject));
        Empires.Clear(); 
    }

}
