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

public class Attributes : MonoBehaviour {

    public BattleScript Battle; 

    /// <summary>
    /// The empire this belongs to, if any. 
    /// </summary>
    public EmpireScript Empire; 

    /// <summary>
    /// The starsystem entity belong to. 
    /// </summary>
    public StarSystem StarSystem; 

    /// <summary>
    /// The tile this entity is on. 
    /// </summary>
    public Vector3Int Location; 

    /// <summary>
    /// The max hp of this entity. 
    /// </summary>
    public int MaxHP;
    /// <summary>
    /// Max armor of this entity. 
    /// </summary>
    public int MaxArmor;
    /// <summary>
    /// max shields of this entity. 
    /// </summary>
    public int MaxShields; 

    /// <summary>
    /// The current hp of this entity. 
    /// </summary>
    public float HP;

    /// <summary>
    /// The current armor. 
    /// </summary>
    public float Armor;

    /// <summary>
    /// The current shields. 
    /// </summary>
    public float Shields;



    /// <summary>
    /// How far this entity can see. 
    /// </summary>
    public int VisionRange;


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
    /// Causes this entity to take damage. 
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="amount"></param>
    public void TakeDamage(DamageType damage, float amount)
    {
        if (Shields > 0)
        {
            switch(damage)
            {
                case DamageType.Energy:
                    Shields -= amount * 1.5f;
                    break;
                default:
                    Shields -= amount;
                    break; 
            }
            return; 
        }
        if (Armor > 0)
        {
            switch(damage)
            {
                case DamageType.Mass:
                    Armor -= amount * 1.5f;
                    break;
                default:
                    Armor -= amount;
                    break; 
            }
        }

        HP -= amount; 
    }

    /// <summary>
    /// Initializes this entities stuff. 
    /// </summary>
    /// <param name="components"></param>
    public void Initialize(int maxHp, List<BaseComponent> components)
    {
        BaseComponents = components.Select(x => x.Clone()).ToList();
        Weight = 0; 
        BaseComponents.ForEach(x => Weight += x.Weight); 

        Weapons = BaseComponents.OfType<WeaponComponent>().ToList();
        Weapons.ForEach(x => TickControlScript.Cooldowns.Add(x));

        MaxHP = maxHp;
        MaxShields = 0;
        MaxArmor = 0; 
        BaseComponents.OfType<ShieldComponent>().ToList().ForEach(x => MaxShields += x.ShieldValue);
        BaseComponents.OfType<ArmorComponent>().ToList().ForEach(x => MaxArmor += x.ArmorValue);
        Engines = 0; 
        BaseComponents.OfType<EngineComponent>().ToList().ForEach(x => Engines += x.Power);
        VisionRange = BaseComponents.OfType<SensorComponent>().Count(); 

        HP = MaxHP;
        Armor = MaxArmor;
        Shields = MaxShields; 

        Movement = (int)( Weight / Engines); 

    }
}
