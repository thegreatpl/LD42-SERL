using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface ICooldown
{
    /// <summary>
    /// The cooldown
    /// </summary>
    int CoolDown { get; }

    /// <summary>
    /// Called at the end of the tick. 
    /// </summary>
    void EndTick(); 

}

