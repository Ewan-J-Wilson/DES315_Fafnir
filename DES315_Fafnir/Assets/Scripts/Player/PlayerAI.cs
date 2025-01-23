using System;
using UnityEngine;
public enum PCom_t
{
    P_NULL,                                     //Null command [Keep as otherwise clones will keep moving left]
    P_LEFT,                                     //Move left
    P_RIGHT,                                    //Move right
    P_JUMP,                                     //Jump
    P_ACTION,                                   //One button action (lever flips)

    P_END,                                      //End command to stop AI from doing other actions
}

public struct PCom
{
    public PCom_t type;                         //Type of command
    public float dur;                           //How long to hold button down
}

public class PlayerAI : MonoBehaviour
{
    public static bool ClearList = false;       //Handshake to ensure the new clone has recieved the command data
    public GameObject Clone;                    //Clone gameobject reference
    string[] ComIdent =                         //List of commands, set up in corresponding order to PCom_t
    {
        "left",
        "right",
        "space",
    };

    protected const int MaxComSize = 8192;      //Maximum amount of commands within the PCList
    protected const int MaxClones = 4;          //Maximum number of clones on screen at once
    protected const float MoveSpeed = 5.0f;     //Constant movement speed
    protected const float JumpForce = 20.0f;    //Constant jump force
    public PCom[] PCList;                       //List of commands for a clone to follow, recorded by player actions
    private PCom CurrentCom;                    //Current command being input by player
    private PCom LastCom;                       //Previous command being input by player
    private int CloneNo;                        //Count of currently spawned clones
    private int ComInd;                         //Index into written commands

    protected Rigidbody2D Rb;                   //Rigidbody for player physics
    protected Vector2 Vel;                      //Movement vector
    protected Vector3 LastPos;                  //Last position the player was at prior to clone creation

    void Start()
    {
        CloneNo = 0;                            //Reset clone amount
        Rb = GetComponent<Rigidbody2D>();
        Array.Resize(ref PCList, 1);            //Resize array to have one element
        ComInd = 0;                             //Reset command index
        LastPos = transform.position;           //Grab current position for future clone position
    }

    void Update()
    {
        HandleMovement();
        transform.Translate(Vel * Time.deltaTime * MoveSpeed);
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
        HandleCommandInput();

        //Debug for command type and duration
        Debug.Log("Type: " + CurrentCom.type + "\nDur: " + CurrentCom.dur);

        //Create clone entity
        if (Input.GetKeyDown("q"))
        {
            //Append END flag into command stream to make sure the clone stops
            PCom end;
            end.type = PCom_t.P_END;
            end.dur = 0;
            PCList[ComInd] = end;
            ComInd++;
            Array.Resize(ref PCList, ComInd+1);
            AddClone();
        }
        //L/R input
        Vel.x = Input.GetAxisRaw("Horizontal");
        
        //Check if the player is on still ground and the spacebar is pressed to jump
        if (Input.GetKeyDown("space") && Rb.velocity.y == 0.0f)
        {
            Rb.velocityY += JumpForce;
        }
    }

    //Create clone
    void AddClone()
    {
        if (CloneNo >= MaxClones)
        {
            return;                                         //Break out of function if the clone limit has been reached
        }
        CloneNo++;
        Instantiate(Clone, LastPos, Quaternion.identity);   //Otherwise we create a clone at the player's last position
        LastPos = transform.position;                       //Then grab the current position for the next future clone
    }

    void HandleCommandInput()
    {
        //Grab the last command for future testing
        LastCom = CurrentCom;
        bool isnull = true;                                 //Flag for if the current key pressed is NOT one of the player's controls
        for (int x = 0; x < 3; x++)
        {
            if (Input.GetKey(ComIdent[x]))
            {
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
                    default:
                        CurrentCom.type = PCom_t.P_NULL;
                        break;
                }
                CurrentCom.dur += Time.deltaTime;           //Add duration onto the current command
            }
        }
        if (isnull)                                         //Add duration if the current key is not a player command
        {
            CurrentCom.dur += Time.deltaTime;
        }

        if (CurrentCom.type != LastCom.type)                //Check for command change
        {
            PCList[ComInd] = LastCom;                       //If a new command is found then we add to the command array
            ComInd++;
            Array.Resize(ref PCList, ComInd + 1);
            CurrentCom.dur = 0;
        }
    }
}
