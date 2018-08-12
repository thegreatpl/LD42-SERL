using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class BaseAttributes : MonoBehaviour
{
    /// <summary>
    /// The type of this entity. 
    /// </summary>
    public string Type; 

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


     void Update()
    {
        if (HP < 0)
            Destroy(gameObject); 
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
            switch (damage)
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
            switch (damage)
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
}

