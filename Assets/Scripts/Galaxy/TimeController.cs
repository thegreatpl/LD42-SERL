using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

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

            var thisTick = TimeObjects.Where(x => x != null && x.CoolDown <= 0);
            int updated = 0; 

            foreach(var entity in thisTick)
            {
                entity.RunTick(); 
                if (updated > MaxUpdate)
                {
                    updated = 0;
                    yield return null; 
                }
            }

            TimeObjects.ForEach(x => x.EndTick());
            EntityManager.EndTick(); 
            yield return new WaitForSeconds(0.5f); 
        }
    }
}
