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
    private float ButtonTimer;          //Time to wait for button to press down
    bool IsPressed = false;             //flag for if the button is pressed
    bool trigger = false;

    private void Start()
    {
        Array.Resize(ref DoorInitState, Doors.Length);
        ButtonPos = transform.position;
        PressedPos = ButtonPos + PressedPos;
    }

    private void Update()
    {

        // Updates the button timer 
        if (IsPressed && ButtonTimer != 1f)
        { ButtonTimer = Mathf.Clamp(ButtonTimer += Time.deltaTime * PressSpeed, 0.0f, 1.0f); }
        else if (!IsPressed && ButtonTimer != 0f)
        { ButtonTimer = Mathf.Clamp(ButtonTimer -= Time.deltaTime * PressSpeed, 0.0f, 1.0f); }

        // The frame the button is pressed down
        if (ButtonTrans.position == PressedPos && (!trigger && IsPressed))
        {
            // Increment the counter for all connected doors
            for (int i = 0; i < Doors.Length; i++)
            {
                Doors[i].GetComponent<OpenObject>().counter++;
                trigger = true;
            }
        }
        // The frame the button is released
        else if (trigger && !IsPressed)
        {
            // Decrement the counter for all connected doors
            for (int i = 0; i < Doors.Length; i++)
            {
                Doors[i].GetComponent<OpenObject>().counter--;
                trigger = false;
            }
        }
    }

    // Animates the button being pressed
    private void FixedUpdate()
    { ButtonTrans.position = Vector3.MoveTowards(ButtonPos, PressedPos, ButtonTimer); }

    // Detects the player on the button
    private void OnTriggerStay2D(Collider2D collision) { 
        if (!(collision.tag == "Player" || collision.tag == "Clone")) 
        { return; }
        IsPressed = true; 
    }

    // Detects the player leaving the button
    private void OnTriggerExit2D(Collider2D collision) { 
        if (!(collision.tag == "Player" || collision.tag == "Clone")) 
        { return; }
        IsPressed = false; 
    }
}
