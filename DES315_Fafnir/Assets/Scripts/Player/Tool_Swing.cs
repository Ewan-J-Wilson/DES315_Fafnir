using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Tool_Swing : MonoBehaviour
{    
    // Designer-controlled hit frame and cooldown times for the tool
    [Range(0.1f, 1f)][SerializeField] protected float hitActive = 0.2f;
    [Range(0.1f, 5f)][SerializeField] protected float hitCooldown = 0.5f;
    // Timers for the hit frames and cooldown
    protected float hitTimer = 0f;
    protected float cooldownTimer = 0f;

    protected PlayerInput input;

    private PlayerAI parent;
    private bool isClone = false;

    float angle = 0;
    Vector2 mouseDirection = new(0,0);

    Vector2 joystickAxis;

    private void Start() 
    {       

        input = gameObject.GetComponentInParent<PlayerInput>();
        parent = (isClone = GetComponentInParent<Rigidbody2D>().CompareTag("Clone"))
                ? GetComponentInParent<CloneAI>()
                : GetComponentInParent<PlayerAI>();

    }

    public void GetJoystickDir(InputAction.CallbackContext obj) {

        if (input.currentControlScheme != "Gamepad" || !obj.performed)
        { return; }
        joystickAxis = obj.ReadValue<Vector2>();

    }

    public void ToolAction(InputAction.CallbackContext obj) {

        if (parent.CurrentCom.tool = obj.performed)
        { SetToolActive(true); }

    }

    // Update is called once per frame
    private void Update()
    {
        //Update tool rotation and activation
        HandleTool();
        // Run any active timers
        RunTimer();
    }

    //Handles the direction of the tool and the activation routines
    protected virtual void HandleTool()
    {
        
        

        // If it's a clone, get the rotation from the current command
        if (isClone) 
        { transform.rotation = Quaternion.AngleAxis(parent.ToolRotation(), Vector3.forward); } 
        // Otherwise get the rotation from the inputs
        else {

            if (GetComponentInParent<PlayerInput>().currentActionMap.name != "Player")
            { return; }

            
            if (input.currentControlScheme == "Keyboard") {

                // Grabs the line between the player centre and the mouse position
                mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;
                // Calculates the angle of that line
                angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x);

            }
            // If using controller, get the angle of the right joystick
            else if (input.currentControlScheme == "Gamepad") 
            { angle = Mathf.Atan2(joystickAxis.y, joystickAxis.x); }
            else
            { Debug.Log("No Input Type Detected"); }

            float circleRadius = transform.parent.GetComponent<CircleCollider2D>().radius;
            
            float angleCos = Mathf.Cos(angle) * circleRadius;
            float angleSin = Mathf.Sin(angle) * circleRadius;
            bool range = (Mathf.Abs(mouseDirection.x) >= Mathf.Abs(angleCos)) || 
                (Mathf.Abs(mouseDirection.y) >= Mathf.Abs(angleSin));

            transform.localPosition = (range ? new Vector3(angleCos,angleSin) : mouseDirection);


        }

    }

    public void SetToolActive(bool _active)
    {
        // Set the tool hitbox to be active
        // Or reset the tool to inactive
        //TODO:

        // Start the hit frame or cooldown timer
        if (_active)
        { hitTimer = hitActive; }
        else
        { cooldownTimer = hitCooldown; }
    }

    protected void RunTimer() {

        // Tick down an active hit frames timer
        if (hitTimer > 0) {

            hitTimer -= Time.deltaTime;

            // If the timer ran out
            if (hitTimer <= 0)
            {
                SetToolActive(false);
            }
        }
        // Tick down an active cooldown timer
        if (cooldownTimer > 0) 
        { cooldownTimer -= Time.deltaTime; }

    }
}
