using System;
using UnityEngine;
using UnityEngine.InputSystem;


// List of current clone relevant actions pressed
// cause for some reason this doesn't exist within PlayerInput
public struct ActionList {
	// horizontal movement
	public float hAxis;
	public bool jump;
	public bool tool;

	public float toolRotation;
	public float dur;
}

public class PlayerAI : MonoBehaviour
{
	
	// Clones
	[SerializeField] private GameObject Clone;  //Clone gameobject reference
	private int CloneNo;                        //Count of currently spawned clones
	protected const int MaxClones = 4;          //Maximum number of clones on screen at once

	// Trails
	public GameObject TrailPart;                //Trail particle reference
	private float TrailTimer;                   //Timer to wait for instantiating a trail when the player is recording
	protected const float MaxTrailTime = 0.15f; //Constant threshold for trails

	// Commands
	public ActionList[] PCList;                 //List of commands for a clone to follow, recorded by player actions
	[NonSerialized] 
	public ActionList CurrentCom;               //Current command being input by player
	private ActionList LastCom;                 //Previous command being input by player
	private int ComInd;                         //Index into written commands
	protected const int MaxComSize = 8192;      //Maximum amount of commands within the PCList
	public static bool ClearList = false;       //Handshake to ensure the new clone has recieved the command data
	private bool IsRecording;                   //Flag to enable movement recording with the player

	// Player
	protected Rigidbody2D Rb;                   //Rigidbody for player physics
	protected Vector2 Vel;                      //Movement vector
	protected Vector3 LastPos;                  //Last position the player was at prior to clone creation
	[SerializeField] [Range(1f, 10f)]
	protected const float MoveSpeed = 7.5f;     //Constant movement speed
	protected const float JumpForce = 25.0f;    //Constant jump force
	protected const float XAccel = 3.33f;		//Constant aceleration
	

    void Start()
	{

		PCList = new ActionList[MaxComSize];
		CloneNo = 0;                            //Reset clone amount
		Rb = GetComponent<Rigidbody2D>();
		Array.Resize(ref PCList, 1);            //Resize array to have one element
		ComInd = 0;                             //Reset command index
		LastPos = transform.position;           //Grab current position for future clone position

	}

	// Records the jump action
    public void JumpAction(InputAction.CallbackContext obj) {
        
		if ((CurrentCom.jump = obj.performed) && Rb.velocityY == 0f) 
		{ Rb.velocityY += JumpForce; }

    }

	// Rounded to ceiling to prevent action being recorded halfway through a stick movement
	public void MoveAction(InputAction.CallbackContext obj) 
	{ CurrentCom.hAxis = Mathf.Ceil(obj.ReadValue<float>()); }

	// Records the clone action
	public void CloneAction(InputAction.CallbackContext obj) {

		// Returns if this is anything but the start of the button press
		if (!obj.started)
		{ return; }

		if (IsRecording)
		{
			if (CloneNo >= MaxClones)
			{
				return;                                         //Break out of function if the clone limit has been reached
			}
			//Load in last command into stream
			PCList[ComInd] = LastCom;
			ComInd++;
			Array.Resize(ref PCList, ComInd + 1);
			//Append END flag into command stream to make sure the clone stops
			ActionList end = new(){ hAxis = 0f, jump = false, tool = false, toolRotation = CurrentCom.toolRotation, dur = 0f };
			PCList[ComInd] = end;
			ComInd++;
			AddClone();
			KillTrail();
			IsRecording = false;
		}
		// Enable the recording
		else
		{
			LastPos = transform.position;
			IsRecording = true;
		}

	}


    protected void Update()
	{
		HandleMovement();
	}

	protected virtual void HandleMovement()
	{
		//Clear command list for next clone once newly made clone has grabbed all current commands
		if (ClearList)
		{
			Array.Resize(ref PCList, 1);
			PCList[0].dur = 0;
			ComInd = 0;
			ClearList = false;
		}

		if (IsRecording)
		{
			HandleCommandInput();
			HandleTrail();
		}

        // L/R input
        Vel.x = Mathf.MoveTowards(Vel.x, MoveSpeed * CurrentCom.hAxis, XAccel);

	}

	protected void FixedUpdate()
	{
		transform.Translate(Vel * Time.deltaTime);
	}

	// Create clone
	void AddClone()
	{
		//Debug.Log("Player pos: " + transform.position);
		CloneNo++;
		Instantiate(Clone, LastPos, Quaternion.identity);   //Otherwise we create a clone at the player's last position
		transform.position = LastPos;                       //Reset player back to original position before recording
	}

	public virtual void KillClone() {

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Clone"))
        {
            Destroy(obj);
        }
		CloneNo = 0;
    }

	private void HandleCommandInput()
	{
		//Check for command change
		if (CurrentCom.hAxis != LastCom.hAxis 
			|| CurrentCom.jump != LastCom.jump 
			|| CurrentCom.tool != LastCom.tool)
		{
			LastCom.toolRotation = GetComponentInChildren<Tool_Swing>().transform.eulerAngles.z % 360;
			PCList[ComInd] = LastCom;                       //If a new command is found then we add to the command array
			ComInd++;
			Array.Resize(ref PCList, ComInd + 1);
			CurrentCom.dur = 0;
		}
		CurrentCom.dur += Time.deltaTime;

		// Grab the last command for future testing
		LastCom = CurrentCom;

	}

	private void HandleTrail()
	{
		if (CloneNo >= MaxClones)
		{
			return;                                         //Break out of function if the clone limit has been reached
		}
		TrailTimer += Time.deltaTime;
		if (TrailTimer > MaxTrailTime)
		{
			Instantiate(TrailPart, transform.position, Quaternion.identity);
			TrailTimer = 0;
		}
	}

	private void KillTrail()
	{
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TrailEnt"))
		{
			Destroy(obj);
		}
	}

	public virtual float ToolRotation()
	{ return CurrentCom.toolRotation; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "MovablePlatform") { transform.parent = collision.transform; }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "MovablePlatform") { transform.parent = null; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "AntiClone") { KillClone(); }
    }
}
