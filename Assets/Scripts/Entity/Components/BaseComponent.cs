using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity.Components
{
    [Serializable]
    public abstract class BaseComponent
    {
        public string Name;

        public List<string> Types;

        public float Weight; 

        public abstract BaseComponent Clone(); 
    }
}
