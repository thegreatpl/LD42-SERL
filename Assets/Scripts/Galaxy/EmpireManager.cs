using Assets.Scripts.Empire;
using Assets.Scripts.Entity.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpireManager : MonoBehaviour {

    public StarSystem StarSystem;

    public Spritemanager Spritemanager; 

    public List<EmpireScript> Empires = new List<EmpireScript>();

    public EntityManager EntityManager;

    public Dictionary<string, ShipDesign> BasicDesigns = new Dictionary<string, ShipDesign>(); 

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
                new WeaponComponent()
                {
                    Amount = 1, CoolDownTime = 3, DamageType = DamageType.Mass, Name = "Cannon", Weight = 0.5f
                }, 
                new EngineComponent()
                {
                    Power = 1, Weight = 0.5f
                }, 
            }
        };
    }


}
