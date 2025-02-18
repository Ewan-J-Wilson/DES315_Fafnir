using System;
using UnityEditor;
using UnityEngine;

public class TriggerButton : TriggerGeneric
{
    private const float PressSpeed = 2f;//Speed at which the button is pressed down
    private Transform ButtonTrans;
    private Vector3 ButtonPos;          //Original position of button
    private Vector3 PressedPos;         //Position for button to go to when fully pressed
    private float ButtonTimer;          //Time to wait for button to press down
    bool IsPressed = false;             //flag for if the button is pressed
    bool trigger = false;

    private void Start()
    {
        ButtonTrans = GetComponentInChildren<Transform>();
        ButtonPos = transform.position;
        PressedPos = ButtonPos + new Vector3(0, -0.5f, 0);
    }

    private void Update()
    {

        // Updates the button timer 
        if (IsPressed && ButtonTimer != 1f)
        { ButtonTimer = Mathf.Clamp(ButtonTimer += Time.deltaTime * PressSpeed, 0, 1); }
        else if (!IsPressed && ButtonTimer != 0f)
        { ButtonTimer = Mathf.Clamp(ButtonTimer -= Time.deltaTime * PressSpeed, 0, 1); }

        // The frame the button is pressed down
        if (ButtonTrans.position == PressedPos && (!trigger && IsPressed)) { 
            OnTrigger();
            trigger = true;
        }
        // The frame the button is released
        else if (trigger && !IsPressed) { 
            OnExit(); 
            trigger = false;
        }

    }

    // Animates the button being pressed
    private void FixedUpdate()
    { ButtonTrans.position = Vector3.MoveTowards(ButtonPos, PressedPos, ButtonTimer); }

    // Detects the player on the button
    private void OnTriggerStay2D(Collider2D collision)
    { if (collision.CompareTag("Player") || collision.CompareTag("Clone")) IsPressed = true; }

    // Detects the player leaving the button
    private void OnTriggerExit2D(Collider2D collision)
    { if (collision.CompareTag("Player") || collision.CompareTag("Clone")) IsPressed = false; }
}
