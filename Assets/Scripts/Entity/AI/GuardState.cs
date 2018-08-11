using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity.AI
{
    public class GuardState : BaseState
    {
        public GuardState(EntityBrain brain) : base(brain) { }

        public override void OnChange()
        {
        }

        public override void OnSet()
        {
        }

        public override void Run()
        {
            if (Brain.Attributes.Battle != null)
                return; 


        }
    }
}
