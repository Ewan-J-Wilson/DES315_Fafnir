using System;
using UnityEngine;
public enum PCom_t
{
	P_NULL,                                     //Null command [Keep as otherwise clones will keep moving left]
	P_LEFT,                                     //Move left
	P_RIGHT,                                    //Move right
	P_JUMP,                                     //Jump
	P_ACTION,                                   //Clone using tool [button activation]

	P_END,                                      //End command to stop AI from doing other actions
}

[System.Serializable]
public struct PCom
{
	public PCom_t type;                         //Type of command
	public float dur;                           //How long to hold button down
	public float val;                           //Generic input value for commands
	public float angl;                          //Angle input for the player tool
}

public class PlayerAI : MonoBehaviour
{
	public static bool ClearList = false;       //Handshake to ensure the new clone has recieved the command data
	public GameObject Clone;                    //Clone gameobject reference
	public GameObject TrailPart;                //Trail particle reference
	public GameObject Tool;                     //Trail particle reference
	
	protected const int MaxComSize = 8192;      //Maximum amount of commands within the PCList
	protected const int MaxClones = 4;          //Maximum number of clones on screen at once
	[SerializeField]
	protected const float MoveSpeed = 5.0f;     //Constant movement speed
	protected const float JumpForce = 20.0f;    //Constant jump force
	protected const float XAccel = 3.33f;		//Constant aceleration
	protected const float MaxTrailTime = 0.15f; //Constant threshold for trails

	public PCom[] PCList;                       //List of commands for a clone to follow, recorded by player actions
	private PCom CurrentCom;                    //Current command being input by player
	private PCom LastCom;                       //Previous command being input by player
	private int CloneNo;                        //Count of currently spawned clones
	private int ComInd;                         //Index into written commands
	private bool IsRecording;                   //Flag to enable movement recording with the player
	private float TrailTimer;                   //Timer to wait for instantiating a trail when the player is recording

	protected Rigidbody2D Rb;                   //Rigidbody for player physics
	protected Vector2 Vel;                      //Movement vector
	protected Vector3 LastPos;                  //Last position the player was at prior to clone creation

	private bool DEBUG_MovementSwitch;          //Switch between different movement types

	KeyCode[] ComIdent =                        //List of commands, set up in corresponding order to PCom_t
	{
		KeyCode.A,
		KeyCode.D,
		KeyCode.Space,
	};
	void Start()
	{
		PCList = new PCom[MaxComSize];
		CloneNo = 0;                            //Reset clone amount
		Rb = GetComponent<Rigidbody2D>();
		Array.Resize(ref PCList, 1);            //Resize array to have one element
		ComInd = 0;                             //Reset command index
		LastPos = transform.position;           //Grab current position for future clone position
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

		//Debug for command type and duration
		//qDebug.Log("Type: " + CurrentCom.type + "\nDur: " + CurrentCom.dur + "\nVal: " + CurrentCom.val);

		//Create clone entity
		if (Input.GetKeyDown("q"))
		{
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
				PCom end;
				end.type = PCom_t.P_END;
				end.dur = 0;
				end.val = 0;
				end.angl = 0;
				PCList[ComInd] = end;
				ComInd++;
				AddClone();
				KillTrail();
				IsRecording = false;
			}
			else
			{
				LastPos = transform.position;
				IsRecording = true;
			}
		}
		if (Input.GetKeyDown("w")) DEBUG_MovementSwitch = !DEBUG_MovementSwitch;
        //L/R input
        Vel.x = Mathf.MoveTowards(Vel.x, MoveSpeed * Input.GetAxis("Horizontal"), XAccel);

        //if (DEBUG_MovementSwitch)
        //{
        //	Vel.x = Mathf.MoveTowards(Vel.x, MoveSpeed * Input.GetAxis("Horizontal"), MoveSpeed * Time.deltaTime);
        //}
        //else
        //{
        //	Vel.x = MoveSpeed * Input.GetAxis("Horizontal");
        //}

        //Check if the player is on still ground and the spacebar is pressed to jump
        if (Input.GetKeyDown("space") && Rb.velocity.y == 0.0f)
		{
			Rb.velocityY += JumpForce;
		}
	}
	protected void FixedUpdate()
	{
		transform.Translate(Vel * Time.deltaTime);
	}

	//Create clone
	void AddClone()
	{
		Debug.Log("Player pos: " + transform.position);
		CloneNo++;
		Instantiate(Clone, LastPos, Quaternion.identity);   //Otherwise we create a clone at the player's last position
		transform.position = LastPos;                       //Reset player back to original position before recording
	}

	private void HandleCommandInput()
	{
		//Grab the last command for future testing
		LastCom = CurrentCom;
		bool isnull = true;                                 //Flag for if the current key pressed is NOT one of the player's controls
		for (int x = 0; x < ComIdent.Length; x++)
		{
			if (Input.GetMouseButtonDown(0))
			{
				CurrentCom.type = PCom_t.P_ACTION;
				isnull = false;
                CurrentCom.val = Vel.x;
                continue;
			}
			if (Input.GetKey(ComIdent[x]))
            {
                CurrentCom.val = Vel.x;
                isnull = false;
				switch (x)                                  //Command type decider
				{
					case 0:
						CurrentCom.type = PCom_t.P_LEFT;
						break;
					case 1:
						CurrentCom.type = PCom_t.P_RIGHT;
						break;
					case 2:
						CurrentCom.type = PCom_t.P_JUMP;
                        break;
				}
			}
		}

		if (isnull)
		{
			CurrentCom.type = PCom_t.P_NULL;
			CurrentCom.val = 0;
		}

		if (CurrentCom.type != LastCom.type)                //Check for command change
		{
			LastCom.angl = Tool.transform.eulerAngles.z % 360;
			PCList[ComInd] = LastCom;                       //If a new command is found then we add to the command array
			ComInd++;
			Array.Resize(ref PCList, ComInd + 1);
			CurrentCom.dur = 0;
		}
		CurrentCom.dur += Time.deltaTime;
	}

	//public void KillClone(GameObject clone)
	//{
	//	if (CloneNo <= 0)
	//	{
	//		return;
	//	}
	//	Destroy(clone);
	//	CloneNo--;
	//}

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
			GameObject.Destroy(obj);
		}
	}
}
