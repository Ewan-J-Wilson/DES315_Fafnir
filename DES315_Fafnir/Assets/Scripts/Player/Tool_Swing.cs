using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Tool_Swing : MonoBehaviour
{    
    // Designer-controlled hit frame and cooldown times for the tool
    [Tooltip("Time the cursor is active")]
    [Range(0.1f, 1f)][SerializeField] protected float hitActive = 0.2f;
    [Tooltip("Cooldown until the cursor can next be used")]
    [Range(0.1f, 5f)][SerializeField] protected float hitCooldown = 0.5f;
    [HideInInspector]
    public List<GameObject> interactables;
    // Timers for the hit frames and cooldown
    protected float hitTimer = 0f;
    protected float cooldownTimer = 0f;

    protected PlayerInput input;

    private PlayerAI parent;
    private bool isClone = false;

    private float angle = 0;
    private Vector2 mouseDirection = new(0,0);

    private Vector2 joystickAxis;

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
        { SetToolActive(); }

    }

    // Update is called once per frame
    private void Update()
    {

        float distance = transform.parent.GetComponent<CircleCollider2D>().radius;

        foreach (GameObject interact in interactables)
        {
            float tempDistance = Vector2.Distance(transform.position, interact.transform.position);

            // Get a reference to whether the object is selected or not
            ref bool isSelected = ref interact.GetComponent<TriggerGeneric>().selected;

            if (distance >= tempDistance && !isSelected)
            {

                // Deselect all out of range objects
                foreach (GameObject deselect in interactables)
                { deselect.GetComponent<TriggerGeneric>().selected = false; }

                // Select the current object
                isSelected = true;
                
            }

        }
          
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
        { transform.localPosition = parent.ToolPosition(); } 
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

    public void SetToolActive()
    {

        if (hitTimer <= 0 && cooldownTimer <= 0) {

            foreach (GameObject interact in interactables)
            {

                TriggerGeneric trigger = interact.GetComponent<TriggerGeneric>();

                if (trigger.selected)
                {

                    trigger.isActive = !trigger.isActive;
                    if (trigger.isActive)
                    { trigger.OnTrigger(); }
                    else
                    { trigger.OnExit(); }

                    break;

                }

            }

            // Start the hit frame
            hitTimer = hitActive;

        }

        

    }

    protected void RunTimer() {

        // Tick down an active hit frames timer
        if (hitTimer > 0) {

            hitTimer -= Time.deltaTime;

            // If the timer ran out
            if (hitTimer <= 0)
            {
                cooldownTimer = hitCooldown;
            }
        }
        // Tick down an active cooldown timer
        if (cooldownTimer > 0) 
        { cooldownTimer -= Time.deltaTime; }

    }
}
