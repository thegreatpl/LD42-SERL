using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour {

    public int Flash = 20; 

    public StarSystem StarSystem; 

    public Camera Camera; 

    public Text Position; 

    public Text Detail;

    public Movement Movement;

    public bool MouseMode = true;

    protected bool MoveEnabled = false; 

    SpriteRenderer Sprite;

    int flashCount = 0;

    /// <summary>
    /// The player empire. 
    /// </summary>
    public EmpireScript PlayerEmpire; 

	// Use this for initialization
	void Start () {
        Movement = GetComponent<Movement>();
        Movement.StarSystem = StarSystem;
        Movement.Location = StarSystem.WorldToTile(transform.position); 
        Position = StarSystem.Position;
        Detail = StarSystem.Detail;
        Camera = StarSystem.Camera; 

        Sprite = GetComponent<SpriteRenderer>();
        Sprite.sprite = StarSystem.Spritemanager.GetSprite("cursor");
        Sprite.color = StarSystem.Spritemanager.Colors["YELLOW"];

        SetCamPos(); 

       // SetMovement(true, StarSystem.MainMenuScreen); 


    }

	
	// Update is called once per frame
	void Update () {

        if (flashCount > 20)
        {
            Sprite.enabled = !Sprite.enabled;
            flashCount = 0; 
        }
        flashCount++; 



        UpdatePostion();
        UpdateDetail(); 
	}

    private void OnDestroy()
    {
        SetMovement(false, StarSystem.MainMenuScreen); 
        Detail.text = "";
        Position.text = ""; 
    }

    /// <summary>
    /// Updates the position. 
    /// </summary>
    public void UpdatePostion()
    {
        Vector3Int tile; 
        if (MouseMode)
        {
            var mousePos = Input.mousePosition;

            var worldPos = Camera.ScreenToWorldPoint(mousePos);
             tile = StarSystem.WorldToTile(worldPos);

        }
        else
        {
            tile = Movement.Location; 
        }
        string pre = "";
        if (StarSystem.TimeController.Paused)
            pre = "PAUSED: "; 

        Position.text = pre + tile.x + ", " + tile.y + ", " + tile.z;
    }

    public void UpdateDetail()
    {
        var location = StarSystem.GetSpaceTile(Movement.Location);
        string detail = "";
        if (!string.IsNullOrWhiteSpace(location.Name))
            detail += $"Name: {location.Name}{Environment.NewLine}";
        if (!string.IsNullOrWhiteSpace(location.Region))
            detail += $"Region: {location.Region}{Environment.NewLine}";
        if (location.MineralValue != 0)
            detail += $"Minerals: {location.MineralValue}{Environment.NewLine}"; 

        Detail.text = detail; 
    }

    /// <summary>
    /// Toggle whether this can move or not. 
    /// </summary>
    public void ToggleMovement()
    {
        SetMovement(!MoveEnabled, StarSystem.MainMenuScreen); 
    }

    /// <summary>
    /// Set whether this can move. 
    /// </summary>
    public void SetMovement(bool setMove, MenuController menu)
    {
        if (!setMove)
        {
            menu.RemoveButton("cursorNE");
            menu.RemoveButton("cursorE");
            menu.RemoveButton("cursorSE");
            menu.RemoveButton("cursorNW");
            menu.RemoveButton("cursorW");
            menu.RemoveButton("cursorSW");
            MoveEnabled = false; 
        }
        else
        {
            menu.AddButton("cursorNE", "E - Move North East", MoveNE, KeyCode.E);
            menu.AddButton("cursorE", "D - Move East", MoveE, KeyCode.D);
            menu.AddButton("cursorSE", "X - Move South East", MoveSE, KeyCode.X);
            menu.AddButton("cursorNW", "W - Move North West", MoveNW, KeyCode.W);
            menu.AddButton("cursorW", "A - Move West", MoveW, KeyCode.A);
            menu.AddButton("cursorSW", "Z - Move South West", MoveSW, KeyCode.Z);
            MoveEnabled = true;
        }
    }


    public void MoveNE()
    {
        Movement.Move(PointyDirection.NorthEast);
        SetCamPos();
    }

    public void MoveE()
    {
        Movement.Move(PointyDirection.East);
        SetCamPos();
    }

    public void MoveSE()
    {
        Movement.Move(PointyDirection.SouthEast);
        SetCamPos();
    }

    public void MoveNW()
    {
        Movement.Move(PointyDirection.NorthWest);
        SetCamPos();
    }

    public void MoveW()
    {
        Movement.Move(PointyDirection.West);
        SetCamPos();
    }

    public void MoveSW()
    {
        Movement.Move(PointyDirection.SouthWest);
        SetCamPos(); 
    }

    public void SetCamPos()
    {
        Camera.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.transform.position.z); 
    }

    /// <summary>
    /// Sets the cursor to the given position. 
    /// </summary>
    /// <param name="location"></param>
    public void SetPosition(Vector3Int location)
    {
        Movement.Move(location);
        SetCamPos(); 
    }
}
