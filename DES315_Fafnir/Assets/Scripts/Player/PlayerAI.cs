using NUnit;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


// List of current clone relevant actions pressed
// cause for some reason this doesn't exist within PlayerInput
[Serializable]
public struct ActionList {
	// horizontal movement
	public float hAxis;
	public bool jump;
	public bool tool;

	public Vector2 toolPosition;
	public float dur;
}

public class PlayerAI : MonoBehaviour
{
	
	// Clones
	[Tooltip("Reference to the Clone object")]
	[SerializeField] private GameObject Clone;  //Clone gameobject reference
	[NonSerialized]
	public int CloneNo;                        //Count of currently spawned clones
	protected int JumpCount = 0;						//Enables double jumping 
	protected const int MaxClones = 4;          //Maximum number of clones on screen at once
	[Range(1,5)] // [SerializeField]
	protected int MaxJump = 1;			//Makes it so double jumping mechanic can't be exploited infinitely 
	

	// Trails
	[Tooltip("Reference to the Trail Particle object")]
	public GameObject TrailPart;                //Trail particle reference
	public GameObject Anchor;					//Anchor particle reference
	private float TrailTimer;                   //Timer to wait for instantiating a trail when the player is recording
	protected const float MaxTrailTime = 0.15f; //Constant threshold for trails

	// Commands
	[HideInInspector]
	public ActionList[] PCList;                 //List of commands for a clone to follow, recorded by player actions
	[HideInInspector] 
	public ActionList CurrentCom;               //Current command being input by player
	private ActionList LastCom;                 //Previous command being input by player
	private int ComInd;                         //Index into written commands
	protected const int MaxComSize = 8192;      //Maximum amount of commands within the PCList
	public static bool ClearList = false;       //Handshake to ensure the new clone has recieved the command data
	[NonSerialized]
	public bool IsRecording;                   //Flag to enable movement recording with the player

	// Player
	public Rigidbody2D Rb;                   //Rigidbody for player physics
	protected Vector2 Vel;                      //Movement vector
	protected Vector3 LastPos;                  //Last position the player was at prior to clone creation
	[Tooltip("The Speed the Player moves at")]
	[SerializeField] [Range(1f, 10f)]
	protected float MoveSpeed = 7.5f;     //Constant movement speed
	protected const float JumpForce = 25.0f;    //Constant jump force
	protected const float XAccel = 3.33f;		//Constant aceleration
	
	public static PlayerInput inputRef;

	// Pause Menu
	private MenuButtons pauseMenu;

	[SerializeField]
	protected string CloningSound;

	// Animator
	protected Animator playerAnimator;
	protected SpriteRenderer playerRenderer;

    void Start()
	{
		// Unpause the game on start
		pauseMenu = FindFirstObjectByType<MenuButtons>();
		if (pauseMenu != null)
		{ pauseMenu.Pause(false); }
		PCList = new ActionList[MaxComSize];
		CloneNo = 0;                            //Reset clone amount
		Rb = GetComponent<Rigidbody2D>();
		Array.Resize(ref PCList, 1);            //Resize array to have one element
		ComInd = 0;                             //Reset command index
		LastPos = transform.position;           //Grab current position for future clone position
		playerRenderer = GetComponentInChildren<SpriteRenderer>();
		playerAnimator = GetComponentInChildren<Animator>();
		inputRef = GetComponent<PlayerInput>();

	}



	// Records the jump action
    public void JumpAction(InputAction.CallbackContext obj) {
        
		if (JumpCount == 0 && Rb.velocityY != 0)
		{ return; }

		if ((CurrentCom.jump = obj.performed) && JumpCount < MaxJump) 
		{

			JumpCount++;
            Rb.velocityY = JumpForce;

			
		}

    }


	// Rounded to ceiling to prevent action being recorded halfway through a stick movement
	public void MoveAction(InputAction.CallbackContext obj) 
	{ CurrentCom.hAxis = Mathf.Ceil(obj.ReadValue<float>()); }

	// Records the clone action
	public void CloneAction(InputAction.CallbackContext obj) {

		// Returns if this is anything but the start of the button press
		if (!obj.started)
		{ return; }

		//Break out of function if the clone limit has been reached
		if (CloneNo >= MaxClones)
		{ return; }

		if (IsRecording)  {

			//Load in last command into stream
			PCList[ComInd] = LastCom;
			ComInd++;
			Array.Resize(ref PCList, ComInd + 1);
			//Append END flag into command stream to make sure the clone stops
			ActionList end = new(){ hAxis = 0f, jump = false, tool = false, toolPosition = CurrentCom.toolPosition, dur = 0f };
			PCList[ComInd] = end;
			ComInd++;
			AddClone();
			KillTrail();
			IsRecording = false;
		}
		// Enable the recording
		else
		{
			// Create the anchor point
			Instantiate(Anchor, transform.position, Quaternion.identity);
			LastPos = transform.position;
			IsRecording = true;
		}

	}


    protected void Update()
	{
		if (playerRenderer == null)
		{ return; }
		HandleMovement();
	}

	protected virtual void HandleMovement()
	{
		if (!playerAnimator.GetBool("InAir"))
		{ playerAnimator.SetBool("InAir", Rb.velocityY > 0.1f || Rb.velocityY < -0.1f); }
		if (Rb.velocityY == 0f)
		{ JumpCount = 0; }
        
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

		
		playerAnimator.SetFloat("Velocity", Mathf.Abs(Vel.x));


        if (Vel.x > 0f)
		{ playerAnimator.SetFloat("Blend", 0f); }
		else if (Vel.x < 0f)
		{ playerAnimator.SetFloat("Blend", 1f); }

		//Debug.Log(playerAnimator.GetBool("IsFacingLeft"));

		//Debug.Log(Rb.velocityY);
		
    }

	protected void FixedUpdate()
	{ transform.Translate(Vel * Time.deltaTime); }

	// Create clone
	void AddClone()
	{
		//Debug.Log("Player pos: " + transform.position);
		CloneNo++;
		Audiomanager.instance.PlayAudio(CloningSound);
		Instantiate(Clone, LastPos, Quaternion.identity);   //Otherwise we create a clone at the player's last position
		transform.position = LastPos;                       //Reset player back to original position before recording
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
	
		if (!collision.IsTouching(GetComponent<CapsuleCollider2D>()))
		{ return; }

        if (collision.CompareTag("DeathZone"))
        { PlayerDeath(); }
    
	}

	public void KillClone() {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Clone"))
        { Destroy(obj); }
		CloneNo = 0;
    }

	public void PlayerDeath()
	{

		// Kills active clones
		KillClone();

		// Reset puzzle objects
		foreach (TriggerGeneric trigger in FindObjectsByType<TriggerGeneric>(FindObjectsSortMode.None))
		{ trigger.Reset(); }

        // Disables active cloning
		ComInd++;
		ClearList = true;
		Array.Resize(ref PCList, ComInd + 1);
		IsRecording = false;
		KillTrail();

        transform.position = GameObject.FindGameObjectWithTag("StartFlag").transform.position;
    }

	
	private void HandleCommandInput()
	{
		//Check for command change
		if ((CurrentCom.hAxis != LastCom.hAxis || ComInd == 0 )
			|| CurrentCom.jump != LastCom.jump 
			|| CurrentCom.tool != LastCom.tool)
		{
			LastCom.toolPosition = GetComponentInChildren<Tool_Swing>().transform.localPosition;
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
		//Break out of function if the clone limit has been reached
		if (CloneNo >= MaxClones)
		{ return; }

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
		{ Destroy(obj); }
	}

	public virtual Vector2 ToolPosition()
	{ return CurrentCom.toolPosition; }

}
