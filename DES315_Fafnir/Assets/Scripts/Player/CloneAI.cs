using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class CloneAI : PlayerAI
{
    protected int ComInd;       //Index into command list
    protected float ComTimer;   //Timer duration for current command

    void Start()
    {
        transform.position = StartPos;
        Vel = Vector2.zero;
    }

    void Update()
    {
        HandleMovement();
        transform.Translate(Vel * Time.deltaTime * MoveSpeed);
    }
    protected override void HandleMovement()
    {
        ComTimer -= Time.deltaTime;
        switch (PCList[ComInd].type)
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
                break;
        }
    }

    protected void ReadCom()
    {
        ComInd++;
        ComInd %= 0x07FF;
        ComTimer = PCList[ComInd].dur;
    }
}
