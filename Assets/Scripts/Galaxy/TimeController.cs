using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class TimeController : MonoBehaviour {

    public List<TickControlScript> TimeObjects = new List<TickControlScript>();

    /// <summary>
    /// Pauses the game. 
    /// </summary>
    public bool Paused = false;

    public int MaxUpdate = 20; 

    /// <summary>
    /// Corotine that handles time. 
    /// </summary>
    Coroutine TickLoop; 

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
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

            var thisTick = TimeObjects.Where(x => x.CoolDown <= 0);
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
            yield return null; 
        }
    }
}
