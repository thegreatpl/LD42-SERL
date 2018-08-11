using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class BaseAttributes : MonoBehaviour
{
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


}

