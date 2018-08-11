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
    /// <summary>
    /// The max hp of this entity. 
    /// </summary>
    public int MaxHP;

    public int MaxArmor;

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
    /// How fast this can move. 
    /// </summary>
    public int Movement;


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

    public void Initialize(List<BaseComponent> components)
    {
        BaseComponents = components.Select(x => x.Clone()).ToList();

        Weapons = BaseComponents.OfType<WeaponComponent>().ToList();
        Weapons.ForEach(x => TickControlScript.Cooldowns.Add(x)); 

    }
}
