using System;
using UnityEngine;

public class CloneAI : PlayerAI
{
    private PCom[] ComList;             //List of commands
    protected float ComTimer;           //Timer duration for current command
    private int ComPos;
    void Start()
    {
        ComPos = 0;
        Vel = Vector2.zero;
        SetComList();
        Rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        transform.Translate(Vel * Time.deltaTime * MoveSpeed);
    }
    protected override void HandleMovement()
    {
        ComTimer -= Time.deltaTime;
        if (ComTimer <= 0f)
        {
            ReadCom();
        }
        switch (ComList[ComPos].type)
        {
            case PCom_t.P_NULL:
                Vel.x = 0;
                break;
            case PCom_t.P_LEFT:
                Vel.x = -1;
                break;
            case PCom_t.P_RIGHT:
                Vel.x = 1;
                break;
            case PCom_t.P_JUMP:
                if (Rb.velocity.y == 0.0f)
                {
                    Rb.velocityY += JumpForce;
                }
                break;
            case PCom_t.P_ACTION:
                break;
            case PCom_t.P_END:
                Vel.x = 0;
                ComTimer = float.MaxValue;
                break;
        }
    }

    protected void ReadCom()
    {
        ComPos++;
        ComPos &= 0x07FF;
        ComTimer = ComList[ComPos].dur;
    }

    public void SetComList()
    {        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Array.Resize(ref ComList, player.GetComponent<PlayerAI>().PCList.Length+1);
        for (int i = 0; i < player.GetComponent<PlayerAI>().PCList.Length; i++)
        {
            ComList[i] = player.GetComponent<PlayerAI>().PCList[i];
        }
        ClearList = true;
    }
}
