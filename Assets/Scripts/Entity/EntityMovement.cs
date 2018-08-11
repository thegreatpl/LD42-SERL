﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EntityMovement :Movement
{
    Attributes Attributes;
    TickControlScript TickControlScript; 

    private void Start()
    {
        Attributes = GetComponent<Attributes>();
        TickControlScript = GetComponent<TickControlScript>(); 

        Move(Location);
    }

    public override void Move(PointyDirection direction)
    {
        Move(Location.GetVectorInDirection(direction));

    }

    public override bool Move(Vector3Int newLoc)
    {
        if (!base.Move(newLoc))
            return false; 
        var basecost = StarSystem.GetMovementCost(newLoc);

        TickControlScript.CoolDown += basecost + Attributes.Movement;
        Attributes.Location = Location; 

        return true;
    }
}
