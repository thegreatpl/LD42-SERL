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

    Movement Movement;

    public bool MouseMode = true;

    protected bool MoveEnabled = false; 

    SpriteRenderer Sprite;

    int flashCount = 0; 

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

        SetMovement(true); 


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
	}

    private void OnDestroy()
    {
        SetMovement(false); 
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

    /// <summary>
    /// Toggle whether this can move or not. 
    /// </summary>
    public void ToggleMovement()
    {
        SetMovement(!MoveEnabled); 
    }

    /// <summary>
    /// Set whether this can move. 
    /// </summary>
    public void SetMovement(bool setMove)
    {
        if (!setMove)
        {
            StarSystem.MainMenuScreen.RemoveButton("cursorNE");
            StarSystem.MainMenuScreen.RemoveButton("cursorE");
            StarSystem.MainMenuScreen.RemoveButton("cursorSE");
            StarSystem.MainMenuScreen.RemoveButton("cursorNW");
            StarSystem.MainMenuScreen.RemoveButton("cursorW");
            StarSystem.MainMenuScreen.RemoveButton("cursorSW");
            MoveEnabled = false; 
        }
        else
        {
            StarSystem.MainMenuScreen.AddButton("cursorNE", "E - Move North East", MoveNE, KeyCode.E);
            StarSystem.MainMenuScreen.AddButton("cursorE", "D - Move East", MoveE, KeyCode.D);
            StarSystem.MainMenuScreen.AddButton("cursorSE", "X - Move South East", MoveSE, KeyCode.X);
            StarSystem.MainMenuScreen.AddButton("cursorNW", "W - Move North West", MoveNW, KeyCode.W);
            StarSystem.MainMenuScreen.AddButton("cursorW", "A - Move West", MoveW, KeyCode.A);
            StarSystem.MainMenuScreen.AddButton("cursorSW", "Z - Move South West", MoveSW, KeyCode.Z);
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

    void SetCamPos()
    {
        Camera.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.transform.position.z); 
    }
}
