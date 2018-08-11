using Assets.Scripts.Entity.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Empire
{
    public class Fleet
    {

        public List<EntityBrain> Ships = new List<EntityBrain>();

        public Vector3Int Location;

        public Vector3Int HomeBase; 

        public string Tag;

        public string action; 

        public void AddShip(EntityBrain brain)
        {
            Ships.Add(brain);
            brain.fleet = this; 
        }


        public void SummonFleet(Vector3Int location)
        {
            Location = location;
            Ships.ForEach(x => x.SetState(new MoveState(x, location))); 
        }
    }
}
