using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity.Components
{
    [Serializable]
    public class ShieldComponent : BaseComponent
    {
        public int ShieldValue; 

        public override BaseComponent Clone()
        {
            return (BaseComponent)MemberwiseClone(); 
        }
    }
}
