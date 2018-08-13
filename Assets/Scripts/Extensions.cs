using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



public static class Extensions
{
    /// <summary>
    /// Gets a random value from this collection. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T Random<T>(this IEnumerable<T> list)
    {
        if (list.Count() == 0)
            return default(T); 
        return list.ElementAt(UnityEngine.Random.Range(0, list.Count())); 
    }
}

