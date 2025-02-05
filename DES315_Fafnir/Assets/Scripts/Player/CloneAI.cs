using System;
using UnityEngine;

public class CloneAI : PlayerAI
{
	protected float ComTimer;               //Timer duration for current command
	public int ComPos;                   //Position into command array
	private bool EndCom;                    //Flag to end command reading

	void Start()
	{
		ComPos = 0;                         //Reset command position
		SetPCList();                       //Grab player commands
		Rb = GetComponent<Rigidbody2D>();
		EndCom = false;
		Vel.x = PCList[ComPos].val;                 //Reset velocity
	}

	protected override void HandleMovement()
	{
		
		switch (PCList[ComPos].type)       //Actions based on player commands
		{
			
			case PCom_t.P_JUMP:
				if (Rb.velocityY == 0f) 
				{ Rb.velocityY += JumpForce; }
				break;

			case PCom_t.P_END:
				EndCom = true;
				break;

			default:
				break;

		}

		if (ComTimer <= 0f && !EndCom)      //If we've hit < 0 AND the END command has NOT been recieved then we read the next command in
		{
			ReadCom();
		}
		ComTimer -= Time.deltaTime;         //Decrement command duration

		Vel.x = Mathf.MoveTowards(Vel.x, MoveSpeed * PCList[ComPos].val, XAccel);

	}

	//Grab next command and set duration timer
	protected void ReadCom()
	{
		//Debug.Log("ComPos: " + ComPos);
		ComPos++;
		ComPos %= MaxComSize;               //Modulo to max com size to prevent OOB errors
		ComTimer = PCList[ComPos].dur;
	}

	//Grabs current commands from player and transfers them to the clone's command list
	public void SetPCList()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Array.Resize(ref PCList, player.GetComponent<PlayerAI>().PCList.Length);
		for (int i = 0; i < player.GetComponent<PlayerAI>().PCList.Length; i++)
		{
			PCList[i] = player.GetComponent<PlayerAI>().PCList[i];
		}
		ClearList = true;
	}

	// NOTE: on end command the tool rotation goes back to the start position
	public override float ToolRotation() 
    { return Mathf.LerpAngle(PCList[ComPos].angl, PCList[(ComPos+1) % PCList.Length].angl, 1 - (ComTimer / PCList[ComPos].dur)); }

}
