using Assets.Scripts.Empire;
using Assets.Scripts.Entity.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DamageType
{
    None,
    Mass, 
    Energy
}

public class Attributes : BaseAttributes {

    public string Type; 

    /// <summary>
    /// How heavy this is. 
    /// </summary>
    public float Weight; 

    /// <summary>
    /// How fast this can move. 
    /// </summary>
    public int Movement;

    /// <summary>
    /// How much engine power there is. 
    /// </summary>
    public int Engines;

    /// <summary>
    /// Whether or not this ship can colonize. 
    /// </summary>
    public bool CanColonize; 


    /// <summary>
    /// List of base components. 
    /// </summary>
    public List<BaseComponent> BaseComponents = new List<BaseComponent>();

    /// <summary>
    /// List of all weapons attached to this entity. 
    /// </summary>
    public List<WeaponComponent> Weapons = new List<WeaponComponent>();

    /// <summary>
    /// The attached tick control script. 
    /// </summary>
    public TickControlScript TickControlScript; 

	// Use this for initialization
	void Start () {
        TickControlScript = GetComponent<TickControlScript>(); 
	}
	
	// Update is called once per frame
	void Update () {
        if (HP < 0)
            Destroy(gameObject);

        if (Armor < 0)
            Armor = 0;
        if (Shields < 0)
            Shields = 0; 
	}

   

    /// <summary>
    /// Initializes this entities stuff. 
    /// </summary>
    /// <param name="components"></param>
    public void Initialize(ShipDesign shipDesign)
    {
        Type = shipDesign.Type;

        GetComponent<SpriteRenderer>().sprite = shipDesign.Sprite; 

        var components = shipDesign.BaseComponents; 


        BaseComponents = components.Select(x => x.Clone()).ToList();
        Weight = 0; 
        BaseComponents.ForEach(x => Weight += x.Weight); 

        Weapons = BaseComponents.OfType<WeaponComponent>().ToList();
        Weapons.ForEach(x => TickControlScript.Cooldowns.Add(x));

        MaxHP = shipDesign.MaxHp;
        MaxShields = 0;
        MaxArmor = 0; 
        BaseComponents.OfType<ShieldComponent>().ToList().ForEach(x => MaxShields += x.ShieldValue);
        BaseComponents.OfType<ArmorComponent>().ToList().ForEach(x => MaxArmor += x.ArmorValue);
        Engines = 0; 
        BaseComponents.OfType<EngineComponent>().ToList().ForEach(x => Engines += x.Power);
        VisionRange = BaseComponents.OfType<SensorComponent>().Count();

        CanColonize = false; 
        if (BaseComponents.OfType<ColonyComponent>().Count() > 0)
            CanColonize = true; 

        HP = MaxHP;
        Armor = MaxArmor;
        Shields = MaxShields; 

        Movement = (int)( Weight / Engines); 

    }
}
