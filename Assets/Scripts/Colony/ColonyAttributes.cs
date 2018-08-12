using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ColonyAttributes : BaseAttributes
{


    private void Start()
    {
        Type = "Colony"; 
        MaxHP = 100;
        HP = MaxHP;
        Armor = 0;
        Shields = 0; 
    }

}

