using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity.AI
{

    public class ColonizeState : MoveState
    {
        public override string Type { get { return "Colonize"; } }


        public ColonizeState(EntityBrain brain, Vector3Int location) : base(brain, location) { }

        public override void Run()
        {
            if (Brain?.Attributes?.Battle != null)
                return;

            if (Target == Brain?.Movement?.Location)
                Colonize(); 

            Move(); 
        }


        void Colonize()
        {

            Brain?.Attributes?.Empire?.CreateColony(Target, Brain.gameObject);

            Brain?.SetState(new GuardState(Brain));

        }
    }
}
