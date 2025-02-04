using System;
using UnityEngine;

public class CloneAI : PlayerAI
{
	public PCom[] ComList;                  //Array of commands
	protected float ComTimer;               //Timer duration for current command
	public int ComPos;                   //Position into command array
	private bool EndCom;                    //Flag to end command reading
	void Start()
	{
		ComPos = 0;                         //Reset command position
		Vel = Vector2.zero;                 //Reset velocity
		SetComList();                       //Grab player commands
		Rb = GetComponent<Rigidbody2D>();
		EndCom = false;
	}

	protected override void HandleMovement()
	{
		Vel.x = Mathf.MoveTowards(Vel.x, ComList[ComPos].val, XAccel);
		switch (ComList[ComPos].type)       //Actions based on player commands
		{
			case PCom_t.P_NULL:             //Keep around for potential future case
				break;
			case PCom_t.P_LEFT:
				break;
			case PCom_t.P_RIGHT:
				break;
			case PCom_t.P_JUMP:
				if (Rb.velocity.y == 0.0f) Rb.velocityY += JumpForce;
				break;
			case PCom_t.P_ACTION:
				break;
			case PCom_t.P_END:
				EndCom = true;
				break;
		}

		if (ComTimer <= 0f && !EndCom)      //If we've hit < 0 AND the END command has NOT been recieved then we read the next command in
		{
			ReadCom();
		}
		ComTimer -= Time.deltaTime;         //Decrement command duration
	}

	//Grab next command and set duration timer
	protected void ReadCom()
	{
		//Debug.Log("ComPos: " + ComPos);
		ComPos++;
		ComPos %= MaxComSize;               //Modulo to max com size to prevent OOB errors
		ComTimer = ComList[ComPos].dur;
	}

	//Grabs current commands from player and transfers them to the clone's command list
	public void SetComList()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Array.Resize(ref ComList, player.GetComponent<PlayerAI>().PCList.Length);
		for (int i = 0; i < player.GetComponent<PlayerAI>().PCList.Length; i++)
		{
			ComList[i] = player.GetComponent<PlayerAI>().PCList[i];
		}
		ClearList = true;
	}
}
