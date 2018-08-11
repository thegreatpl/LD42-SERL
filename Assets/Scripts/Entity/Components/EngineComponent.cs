using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity.Components
{
    public class EngineComponent : BaseComponent
    {
        /// <summary>
        /// how much power this engine has. 
        /// </summary>
        public int Power;

        public override BaseComponent Clone()
        {
            return (EngineComponent)this.MemberwiseClone(); 
        }
    }
}
