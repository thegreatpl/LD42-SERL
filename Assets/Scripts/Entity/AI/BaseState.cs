using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity.AI
{
    public abstract class BaseState 
    {

        public EntityBrain Brain;


        public BaseState(EntityBrain brain)
        {
            Brain = brain;

        }


        public abstract void OnSet();

        public abstract void Run();

        public abstract void OnChange(); 


    }
}
