﻿using Assets.Scripts.Empire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColonyAction
{
    Mining, 
    Building
}

public class ColonyControl : MonoBehaviour, ITickable {

    public EmpireScript Empire;

    public StarSystem StarSystem;

    public Vector3Int Location; 

    public SpaceTile Colony; 

    public int CoolDown => coolDown;

    public float Buildrate = 1.2f; 

    public int coolDown;


    public ColonyAction ColonyAction; 

    ShipDesign design;
    float buildLeft;

    bool init = false;

    public ColonyAttributes Attributes; 
    // Use this for initialization
    void Start () {

        Colony = StarSystem.GetSpaceTile(Location);
        StarSystem.TimeController.TimeObjects.Add(this);
        var sp = GetComponent<SpriteRenderer>();
        sp.sprite = StarSystem.Spritemanager.GetSprite("colony");
        ColonyAction = ColonyAction.Mining;
        Attributes = GetComponent<ColonyAttributes>(); 
        init = true; 
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
        if (!init)
            return;

        switch(ColonyAction)
        {
            case ColonyAction.Mining:
                Mine(); break;
            case ColonyAction.Building:
                Build(); break; 
        }

        //Regenerate battle damage. 
        if (Attributes.Battle == null && Attributes.HP < Attributes.MaxHP)
            Attributes.HP++; 
        
    }


    public bool BuildShip(ShipDesign design)
    {
        if (ColonyAction != ColonyAction.Building && Empire.Resouces > design.Cost)
        {
            ColonyAction = ColonyAction.Building; 
            this.design = design;
            buildLeft = (design.BaseComponents.Count * 10) + design.MaxHp;
            Empire.Resouces -= design.Cost; 
            return true; 
        }

        return false; 
    }

    void Build()
    {
        if (design != null)
        {
            buildLeft -= Buildrate;
            if (buildLeft < 0)
            {
                Empire.CreateEntity(Location, design);
                design = null;
                ColonyAction = ColonyAction.Mining; 
            }
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
