using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tool_Swing : MonoBehaviour
{
    
    // Sprite colours for the inactive and active tool respectively
    [SerializeField] protected Color ogColour = Color.yellow;
    [SerializeField] protected Color hitColour = Color.red;
    // Designer-controlled hit frame and cooldown times for the tool
    [Range(0.1f, 1f)][SerializeField] protected float hitActive = 0.2f;
    [Range(0.1f, 5f)][SerializeField] protected float hitCooldown = 0.5f;
    // Timers for the hit frames and cooldown
    protected float hitTimer = 0f;
    protected float cooldownTimer = 0f;
    // The tool's sprite
    protected GameObject toolSprite;
    protected PlayerInput input;

    private PlayerAI parent;
    private bool isClone = false;

    Vector2 joystickAxis;


    private void Start() 
    {       

        // Initialise the tool as inactive
        toolSprite = gameObject.GetComponentInChildren<SpriteRenderer>().gameObject;
        toolSprite.GetComponent<SpriteRenderer>().color = ogColour;
        toolSprite.GetComponent<BoxCollider2D>().size = Vector2.zero;

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

            if (input.currentControlScheme == "Keyboard") {

                // Grabs the line between the player centre and the mouse position
                Vector2 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                // Calculates the angle of that line
                float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
                // Update the tool's z rotation to the angle
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            }
            // If using controller, get the angle of the right joystick
            else if (input.currentControlScheme == "Gamepad") {

                float angle = Mathf.Atan2(joystickAxis.y, joystickAxis.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            }
            else
            { Debug.Log("No Input Type Detected"); }

        }

    }

    protected void SetToolActive(bool _active)
    {
        // Set the tool hitbox to be active
        // Or reset the tool to inactive
        toolSprite.GetComponent<SpriteRenderer>().color = _active ? hitColour : ogColour;
        toolSprite.GetComponent<BoxCollider2D>().size = _active ? Vector2.one : Vector2.zero;
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
