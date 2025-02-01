using System;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public struct PComs
{

    // Horizontal Movement Axis
    public float hAxis;

    // Jump Action
    public bool jump;

    // Tool Action
    public bool useTool;
    public float toolRotation;

    // Length of Command
    public float dur;

}

public class PlayerAI : MonoBehaviour
{
    
    // Clones
    public GameObject Clone;                    //Clone gameobject reference
    protected const int MaxClones = 4;          //Maximum number of clones on screen at once
    private int CloneNo;                        //Count of currently spawned clones
    
    // Commands
    public PComs[] PCList;                      //List of commands for a clone to follow, recorded by player actions
    private PComs CurrentCom;                   //Current command being input by player
    private PComs LastCom;                      //Previous command being input by player
    public static bool ClearList = false;       //Handshake to ensure the new clone has recieved the command data
    protected const int MaxComSize = 8192;      //Maximum amount of commands within the PCList
    private int ComInd;                         //Index into written commands
    private bool IsRecording;                   //Flag to enable movement recording with the player

    // Trails
    public GameObject TrailPart;                //Trail particle reference
    private float TrailTimer;                   //Timer to wait for instantiating a trail when the player is recording
    protected const float MaxTrailTime = 0.15f; //Constant threshold for trails

    // Player
    protected Rigidbody2D Rb;                   //Rigidbody for player physics
    protected Vector2 Vel;                      //Movement vector
    protected Vector3 LastPos;                  //Last position the player was at prior to clone creation
    [SerializeField]
    protected const float MoveSpeed = 5.0f;     //Constant movement speed
    protected const float JumpForce = 20.0f;    //Constant jump force


    void Start()
    {
        PCList = new PComs[MaxComSize];
        CloneNo = 0;                            //Reset clone amount
        Rb = GetComponent<Rigidbody2D>();
        Array.Resize(ref PCList, 1);            //Resize array to have one element
        ComInd = 0;                             //Reset command index
        LastPos = transform.position;           //Grab current position for future clone position

        CurrentCom = new PComs(){ hAxis = 0f, jump = false, useTool = false, toolRotation = 0f, dur = 0f};

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

       
        //L/R input
        CurrentCom.hAxis = Input.GetAxisRaw("Horizontal");

        Debug.Log(CurrentCom.hAxis);

        //Check if the player is on still ground and the spacebar is pressed to jump
        if (CurrentCom.jump = Input.GetButton("Jump"))
        { Jump(); }

        if (CurrentCom.useTool = Input.GetButtonDown("Activate"))
        { CurrentCom.toolRotation = GameObject.Find("Tool").transform.eulerAngles.z; }



        if (IsRecording)
        {
            HandleTrail();

            // If the commands have changed, record them
            if (CurrentCom.hAxis != LastCom.hAxis || CurrentCom.jump != LastCom.jump || CurrentCom.useTool != LastCom.useTool) {

                LastCom = CurrentCom;
                ComInd++;
                Array.Resize(ref PCList, ComInd + 1);
                PCList[ComInd] = LastCom;
                
                CurrentCom.dur = 0;

            }
        }

        CurrentCom.dur += Time.fixedDeltaTime;

        //Create clone entity
        if (Input.GetButtonDown("Clone"))
        {
            if (IsRecording)
            {
                //Break out of function if the clone limit has been reached
                if (CloneNo >= MaxClones)
                { return; }

                //Load in last command into stream
                ComInd++;
                Array.Resize(ref PCList, ComInd + 1);
                PCList[ComInd] = LastCom;
                
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
        
        

    }
    protected void FixedUpdate()
    {
        HandleMovement();

        Vel.x = Mathf.MoveTowards(Vel.x, MoveSpeed * CurrentCom.hAxis, MoveSpeed);
        Debug.Log(Vel.x);
        transform.Translate(Vel * Time.fixedDeltaTime);
    }


    protected void Jump() {
        if (Rb.velocityY == 0f)
        { Rb.velocityY += JumpForce; }
    }

    //Create clone
    void AddClone()
    {
        CloneNo++;
        Instantiate(Clone, LastPos, Quaternion.identity);   //Otherwise we create a clone at the player's last position
        transform.position = LastPos;                       //Reset player back to original position before recording
    }

    public void KillClone(GameObject clone)
    {
        if (CloneNo <= 0)
        { return; }

        Destroy(clone);
        CloneNo--;
    }

    private void HandleTrail()
    {

        //Break out of function if the clone limit has been reached
        if (CloneNo >= MaxClones)
        { return; }

        TrailTimer += Time.deltaTime;
        // If time threshold has been reached, make a new trail object and reset timer
        if (TrailTimer > MaxTrailTime)
        {
            Instantiate(TrailPart, transform.position, Quaternion.identity);
            TrailTimer = 0;
        }
    }

    // Kill all trail objects
    private void KillTrail()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TrailEnt"))
        { GameObject.Destroy(obj); }
    }
}
