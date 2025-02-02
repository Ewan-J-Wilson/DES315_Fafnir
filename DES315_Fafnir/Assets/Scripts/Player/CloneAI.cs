using System;
using UnityEngine;

public class CloneAI : PlayerAI
{
    public PComs[] ComList;                  //Array of commands
    protected float ComTimer;               //Timer duration for current command
    private int ComPos;                     //Position into command array
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
       
        ComTimer -= Time.deltaTime;         //Decrement command duration
        if (ComTimer <= 0f && !EndCom)      //If we've hit < 0 AND the END command has NOT been recieved then we read the next command in
        { ReadCom(); }
        else if (!EndCom) {

            if (CurrentCom.jump)
            { Jump(); }

            if (CurrentCom.useTool)
            { CurrentCom.toolRotation = GameObject.Find("Tool").transform.eulerAngles.z; }

            if (ComPos == ComList.Length)
            { EndCom = true; }

        }
    }

    //Grab next command and set duration timer
    protected void ReadCom()
    {
        //Debug.Log("ComPos: " + ComPos);
        ComPos++;
        ComPos %= MaxComSize;               //Modulo to max com size to prevent OOB errors
        CurrentCom = ComList[ComPos];
        ComTimer = CurrentCom.dur;
        
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
