using System;
using UnityEngine;

public class CloneAI : PlayerAI
{
	protected float ComTimer;               //Timer duration for current command
	private int ComPos;						//Position into command array
	private bool EndCom;                    //Flag to end command reading
	private bool JumpCheck = false;

	void Start()
	{
		Vel = Vector2.zero;					//Reset velocity
		ComPos = 0;                         //Reset command position
		SetPCList();						//Grab player commands
		Rb = GetComponent<Rigidbody2D>();
		EndCom = false;
        playerRenderer = GetComponentInChildren<SpriteRenderer>();
        playerAnimator = GetComponentInChildren<Animator>();

    }

	protected override void HandleMovement()
	{
        playerAnimator.SetBool("InAir", Rb.velocityY != 0f);
        if (Rb.velocityY == 0f)
		{ JumpCount = 0; }

		if (!PCList[ComPos].jump)
        { JumpCheck = true; }
        else if (JumpCount < MaxJump && JumpCheck)
        {

            JumpCount++;
            Rb.velocityY = JumpForce;
			JumpCheck = false;

        }

		if (PCList[ComPos].tool)
		{ GetComponentInChildren<Tool_Swing>().SetToolActive(); }

		if (ComPos == PCList.Length - 1)
		{ EndCom = true; }

		if (ComTimer <= 0f && !EndCom)      //If we've hit < 0 AND the END command has NOT been recieved then we read the next command in
		{
			ReadCom();
		}

		ComTimer -= Time.deltaTime;         //Decrement command duration

		Vel.x = Mathf.MoveTowards(Vel.x, MoveSpeed * PCList[ComPos].hAxis, XAccel);
        playerAnimator.SetFloat("Velocity", Mathf.Abs(Vel.x));

		if (Vel.x > 0f)
		{ playerRenderer.flipX = false; }
		else if (Vel.x < 0f)
		{ playerRenderer.flipX = true; }

    }

	//Grab next command and set duration timer
	protected void ReadCom()
	{
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

    public override Vector2 ToolPosition() {
        float lerpX = Mathf.Lerp(PCList[ComPos].toolPosition.x, PCList[(ComPos+1) % PCList.Length].toolPosition.x, 1 - (ComTimer / PCList[ComPos].dur));
        float lerpY = Mathf.Lerp(PCList[ComPos].toolPosition.y, PCList[(ComPos+1) % PCList.Length].toolPosition.y, 1 - (ComTimer / PCList[ComPos].dur));
		
		return new(lerpX,lerpY);
    }

}
