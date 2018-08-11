using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

[Serializable]
public class PrefabDef
{
    public string Name;

    public GameObject GameObject; 
}

public class PrefabManager : MonoBehaviour {

    public List<PrefabDef> Prefabs;

    void Start()
    {
        
    }

    /// <summary>
    /// Gets a prefab from the manage.r 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetPrefab(string name)
    {
        return Prefabs.FirstOrDefault(x => x.Name == name).GameObject; 
    }
}
