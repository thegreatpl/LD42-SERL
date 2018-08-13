using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity.AI
{
    public class MoveState : BaseState
    {
        public override string Type { get { return "Move"; } }


        public  Vector3Int Target;

        protected  Queue<Vector3Int> MovementQueue = new Queue<Vector3Int>(); 

        public MoveState(EntityBrain brain, Vector3Int target) : base(brain) { Target = target; }

        public override void OnChange()
        {
        }

        public override void OnSet()
        {
            GetPath(); 
        }

        public override void Run()
        {
            if (Brain?.Attributes?.Battle != null)
                return; 

            if (Brain?.Movement?.Location == Target)
            {
                Brain?.SetState(new GuardState(Brain));
                return; 
            }

            Move(); 
        }

        public void Move()
        {
            if (MovementQueue.Count < 1)
            {
                GetPath(); 
            }
            //may cause problems. 
            Brain?.Movement?.Move(MovementQueue.Dequeue()); 
        }

        public virtual void GetPath()
        {
            if (Brain == null)
                return; 
            var current = Brain.Movement.Location;
            var curHex = OffsetCoord.RFromUnity(current);
            var tarhex = OffsetCoord.RFromUnity(Target);
            var line = FractionalHex.HexLinedraw(curHex, tarhex);
            foreach (var h in line)
                MovementQueue.Enqueue(OffsetCoord.RToUnityCoords(h)); 
        }
    }
}
