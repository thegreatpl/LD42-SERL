using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TimeController : MonoBehaviour {

    public List<ITickable> TimeObjects = new List<ITickable>();

    /// <summary>
    /// Pauses the game. 
    /// </summary>
    public bool Paused = false;

    public int MaxUpdate = 20; 

    /// <summary>
    /// Corotine that handles time. 
    /// </summary>
    Coroutine TickLoop;


    EntityManager EntityManager; 

	// Use this for initialization
	void Start () {
        EntityManager = GetComponent<EntityManager>(); 
	}
	
	// Update is called once per frame
	void Update () {
        TimeObjects.RemoveAll(x => x == null);
        TimeObjects.RemoveAll(x => x.Equals(null)); 
	}

    public void StartNewGame()
    {
        TimeObjects.Clear();
        StartCoroutine(Tick()); 

    }

    public void EndGame()
    {
        StopAllCoroutines();
        TimeObjects.Clear(); 
    }

    IEnumerator Tick()
    {
        while (true)
        {

            while (Paused)
                yield return null;

            var thisTick = TimeObjects.Where(x => (x != null ||!x.Equals(null)) && x.CoolDown <= 0 ).ToList();
            int updated = 0;

            foreach (var entity in thisTick)
            {
                try
                {
                    if (entity == null)
                        continue;
                    if ((entity is UnityEngine.Object) && (entity.Equals(null)))
                        continue;



                    entity?.RunTick();
                }
                //stop errors from destroying the game entire. 
                catch (Exception e)
                {
                    Debug.LogError($"Error: Exception caught by time controller:{e.Message}");
                }
                updated++; 
                if (updated > MaxUpdate)
                {
                    updated = 0;
                    yield return null;
                }
            }

            TimeObjects.ForEach(x => x.EndTick());
            try
            {
                EntityManager.EndTick();
            }
            catch (Exception e)
            { Debug.LogError($"Error: Entity Manager threw exception during end tick:{e.Message}"); }
            yield return new WaitForSeconds(0.25f);

        }
    }
}
