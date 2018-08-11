using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity.Components
{
    [Serializable]
    public class ColonyComponent : BaseComponent
    {
        public override BaseComponent Clone()
        {
            return (BaseComponent)MemberwiseClone(); 
        }
    }
}
