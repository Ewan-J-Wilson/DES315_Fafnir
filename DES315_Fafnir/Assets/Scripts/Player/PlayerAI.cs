using System;
using UnityEngine;
public enum PCom_t
{
    P_NULL,                                     //No action
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

    protected const int MaxComSize = 2048;      //Maximum numver of clones on screen at once
    protected const int MaxClones = 4;          //Maximum numver of clones on screen at once
    protected const float MoveSpeed = 5.0f;     //Constant movement speed
    protected const float JumpForce = 20.0f;    //Constant jump force
    public PCom[] PCList;                       //List of commands for a clone to follow, recorded by player actions
    private PCom CurrentCom;                    //Current command being input by player
    private PCom LastCom;                       //Previous command being input by player
    private int CloneNo;                        //Count of currently spawned clones
    private int ComInd;                         //Index into written commands

    protected Rigidbody2D Rb;
    protected Vector2 Vel;
    protected Vector3 LastPos;

    void Start()
    {
        CloneNo = 0;
        Rb = GetComponent<Rigidbody2D>();
        Array.Resize(ref PCList, 1);
        ComInd = 0;
        LastPos = transform.position;
    }

    void Update()
    {
        HandleMovement();
        transform.Translate(Vel * Time.deltaTime * MoveSpeed);
    }

    protected virtual void HandleMovement()
    {
        if (ClearList)
        {
            Array.Resize(ref PCList, 1);
            PCList[0].type = PCom_t.P_NULL;
            PCList[0].dur = 0;
            ComInd = 0;

            ClearList = false;
        }
        LastCom = CurrentCom;
        bool isnull = true;
        for (int x = 0; x < 3; x++)
        {
            if (Input.GetKey(ComIdent[x]))
            {
                isnull = false;
                switch (x)
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
                CurrentCom.dur += Time.deltaTime;
            }
        }
        if (isnull)
        {
            CurrentCom.type = PCom_t.P_NULL;
            CurrentCom.dur += Time.deltaTime;
        }

        if (CurrentCom.type != LastCom.type)
        {
            PCList[ComInd] = LastCom;
            ComInd++;
            Array.Resize(ref PCList, ComInd+1);
            CurrentCom.dur = 0;
        }

        Debug.Log("Type: " + CurrentCom.type + "\nDur: " + CurrentCom.dur);

        if (Input.GetKeyDown("q"))
        {
            PCom end;
            end.type = PCom_t.P_END;
            end.dur = 0;
            PCList[ComInd] = end;
            ComInd++;
            Array.Resize(ref PCList, ComInd+1);
            AddClone();
        }
        Vel.x = Input.GetAxisRaw("Horizontal");

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
            return;
        }
        CloneNo++;
        Instantiate(Clone, LastPos, Quaternion.identity);
        LastPos = transform.position;
    }
}
