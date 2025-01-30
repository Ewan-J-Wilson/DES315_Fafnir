using System;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    private const float PressSpeed = 2f;//Speed at which the button is pressed down
    public Transform ButtonTrans;
    private Vector3 ButtonPos;          //Original position of button
    [SerializeField]
    private Vector3 PressedPos;         //Position for button to go to when fully pressed
    public GameObject[] Doors;          //Doors who's state we invert
    private bool[] DoorInitState;       //Initial active/deactive state of doors
    private float ButtomTimer;          //Time to wait for button to press down
    bool IsPressed = false;             //flag for if the button is pressed

    private void Start()
    {
        Array.Resize(ref DoorInitState, Doors.Length);
        ButtonPos = transform.position;
        PressedPos = ButtonPos + PressedPos;
        for (int i = 0; i < Doors.Length; i++)
        {
            DoorInitState[i] = Doors[i].active;
        }
    }

    private void Update()
    {
        if (IsPressed)
        {
            ButtomTimer += Time.deltaTime * PressSpeed;
        }
        else
        {
            ButtomTimer -= Time.deltaTime * PressSpeed;
        }
        ButtomTimer = Mathf.Clamp(ButtomTimer, 0.0f, 1.0f);

        if (ButtonTrans.position == PressedPos)
        {
            for (int i = 0; i < Doors.Length; i++)
            {
                Doors[i].active = !DoorInitState[i];
            }
        }
        else
        {
            for (int i = 0; i < Doors.Length; i++)
            {
                Doors[i].active = DoorInitState[i];
            }
        }
    }

    private void FixedUpdate()
    {
        ButtonTrans.position = Vector3.MoveTowards(ButtonPos, PressedPos, ButtomTimer);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Clone") IsPressed = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Clone") IsPressed = false;
    }
}
