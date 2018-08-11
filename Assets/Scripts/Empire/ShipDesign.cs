using Assets.Scripts.Entity.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Empire
{
    [Serializable]
    public class ShipDesign
    {
        public string Name;

        public string Type;

        public int MaxHp;

        public Sprite Sprite; 

        public List<BaseComponent> BaseComponents = new List<BaseComponent>(); 
    }
}
