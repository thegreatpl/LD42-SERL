using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LogItem
{
    public DateTime Post;

    public string Message;

    public Vector3Int Location;

    public Text Text; 
}

public class LogScript : MonoBehaviour {

    List<LogItem> LogItems = new List<LogItem>();

    /// <summary>
    /// Link to the starsytem game object. 
    /// </summary>
    public StarSystem StarSystem;

    /// <summary>
    /// The text prefab. 
    /// </summary>
    public GameObject TextObj; 
    /// <summary>
    /// The ping location. 
    /// </summary>
    public Vector3Int PingLocation;

    /// <summary>
    /// Link to the prefab manager. 
    /// </summary>
    public PrefabManager PrefabManager;


    public Spritemanager Spritemanager; 


    public int MaxObjects = 10; 

	// Use this for initialization
	void Start () {
        PingLocation = Vector3Int.zero;
        TextObj = PrefabManager.GetPrefab("LogItem");

        Application.logMessageReceived += Application_logMessageReceived;

    }

    /// <summary>
    /// Used to log the messages to the console. 
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        var color = "WHITE"; 
        switch (type)
        {
            case LogType.Error:
                color = "RED";
                break;
            case LogType.Exception:
                color = "MAGENTA";
                break;
            case LogType.Warning:
                color = "YELLOW";
                break;

        }
        //if (type == LogType.Log)
        //    return;
        Log($"{condition}", PingLocation, color); 
    }

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
    /// Clears the log. 
    /// </summary>
    public void ClearLog()
    {
        foreach (var item in LogItems)
            Destroy(item.Text.gameObject); 

        LogItems = new List<LogItem>(); 
    }

    /// <summary>
    /// Removes the oldest log. 
    /// </summary>
    public void RemoveOldest()
    {
        var oldest = LogItems.OrderBy(x => x.Post).FirstOrDefault();

        if (oldest == null)
            return;

        Destroy(oldest.Text.gameObject);
        LogItems.Remove(oldest); 
    }

    /// <summary>
    /// Logs a new message. 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="location"></param>
    public void Log(string message, Vector3Int location, string color = "WHITE")
    {
        var col = Spritemanager.Colors[color]; 

        while (LogItems.Count > MaxObjects)
            RemoveOldest(); 

        var newobj =Instantiate(TextObj, transform);
        var t = newobj.GetComponent<Text>();
        t.text = message;
        t.color = col; 
        LogItems.Add(new LogItem()
        {
            Location = location, Text = t, Message = message, Post = DateTime.Now 
        });
        PingLocation = location; 
    }
}
