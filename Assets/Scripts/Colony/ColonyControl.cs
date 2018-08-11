using Assets.Scripts.Empire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyControl : MonoBehaviour, ITickable {

    public EmpireScript Empire;

    public StarSystem StarSystem;

    public Vector3Int Location; 

    public SpaceTile Colony; 

    public int CoolDown => coolDown;

    public float Buildrate = 1.2f; 

    public int coolDown;

    public bool mining = true;

    public bool building = true; 

    ShipDesign design;
    float buildLeft; 
    

    // Use this for initialization
    void Start () {
        Colony = StarSystem.GetSpaceTile(Location);
        StarSystem.TimeController.TimeObjects.Add(this);
        var sp = GetComponent<SpriteRenderer>();
        sp.sprite = StarSystem.Spritemanager.GetSprite("colony"); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EndTick()
    {
        coolDown--;
        if (coolDown < 0)
            coolDown = 0; 
    }

    public void RunTick()
    {
        if (mining)
            Mine();

        if (building)
            Build(); 
        
    }


    public bool BuildShip(ShipDesign design)
    {
        if (!building && Empire.Resouces > design.Cost)
        {
            mining = false;
            this.design = design;
            buildLeft = (design.BaseComponents.Count * 10) + design.MaxHp;
            Empire.Resouces -= design.Cost; 
            building = true;
            return true; 
        }

        return false; 
    }

    void Build()
    {
        buildLeft -= Buildrate; 
        if (buildLeft < 0)
        {
            Empire.CreateEntity(Location, design);
            building = false; 
            mining = true; 
        }
    }


    /// <summary>
    /// mines minerals from the planet. 
    /// </summary>
    void Mine()
    {
        Empire.Resouces += Colony.MineralValue; 
        coolDown = 5; 
    }
}
