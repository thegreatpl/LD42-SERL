using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity.Components
{
    [Serializable]
    public class ArmorComponent : BaseComponent
    {
        public int ArmorValue; 

        public override BaseComponent Clone()
        {
            return (BaseComponent)MemberwiseClone(); 
        }
    }
}
