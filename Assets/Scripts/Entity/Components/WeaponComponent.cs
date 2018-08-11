using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity.Components
{
    [Serializable]
    public class WeaponComponent : BaseComponent, ICooldown
    {

        public DamageType DamageType;

        public float Amount;

        public int CoolDownTime; 

        public int CoolDown { get; set; }

        public override BaseComponent Clone()
        {
            var r = (WeaponComponent)this.MemberwiseClone();
            r.CoolDown = 0; 
            return r;
        }

        public void EndTick()
        {
            if (CoolDown > 0)
                CoolDown--; 
        }
    }
}
