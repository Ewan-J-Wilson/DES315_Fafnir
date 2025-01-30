using System;
using UnityEngine;

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


    private void Start() {
        
        // Initialise the tool as inactive
        toolSprite = GameObject.Find("Tool Sprite");
        toolSprite.GetComponent<SpriteRenderer>().color = ogColour;
        toolSprite.GetComponent<BoxCollider2D>().size = Vector2.zero;

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
        // Grabs the line between the player centre and the mouse position
        Vector2 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // Calculates the angle of that line
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        // Update the tool's z rotation to the angle
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // If the user is using the tool
        if (Input.GetButtonDown("Activate") && cooldownTimer <= 0f)
        {
            // Set the tool hitbox to be active
            toolSprite.GetComponent<SpriteRenderer>().color = hitColour;
            toolSprite.GetComponent<BoxCollider2D>().size = Vector2.one;
            // Start the hit frame timer
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
                // Reset the tool to inactive
                toolSprite.GetComponent<SpriteRenderer>().color = ogColour;
                toolSprite.GetComponent<BoxCollider2D>().size = Vector2.zero;
                // Start the cooldown timer
                cooldownTimer = hitCooldown;   
            }
        }
        // Tick down an active cooldown timer
        if (cooldownTimer > 0) 
        { cooldownTimer -= Time.deltaTime; }

    }
}
