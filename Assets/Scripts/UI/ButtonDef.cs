using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UI
{
    [Serializable]
    public class PageObjectDef
    {
        public string name;

        public string text;
    }

    [Serializable]
    public class ButtonDef : PageObjectDef
    {
       public OnClick OnClick; 

    }

    [Serializable]
    public class UpdateTextDef :PageObjectDef
    {
        public GetText UpdateText; 
    }
}
