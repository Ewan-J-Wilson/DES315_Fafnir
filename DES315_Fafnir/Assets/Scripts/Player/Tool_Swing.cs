using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
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
    private PlayerAI parent;
    private bool isClone = false;


    protected void Start() {
        
        // Initialise the tool as inactive
        toolSprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        toolSprite.GetComponent<SpriteRenderer>().color = ogColour;
        toolSprite.GetComponent<BoxCollider2D>().size = Vector2.zero;

        parent = (isClone = GetComponentInParent<Rigidbody2D>().CompareTag("Clone"))
                ? GetComponentInParent<CloneAI>()
                : GetComponentInParent<PlayerAI>();

    }

    // Update is called once per frame
    void Update()
    {

        if (isClone) {

            //Mathf.LerpAngle(parent.CurrentCom.toolRotation, parent.NextCom.toolRotation, parent.CurrentCom.dur - parent.ComTimer);
            transform.rotation = Quaternion.AngleAxis(parent.ToolRotation(), Vector3.forward);
            //transform.rotation = Quaternion.AngleAxis(parent.CurrentCom.toolRotation, Vector3.forward);
            Debug.Log(parent.CurrentCom.toolRotation);

        } 
        else {

            // Grabs the line between the player centre and the mouse position
            Vector2 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            // Calculates the angle of that line
            float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
            // Update the tool's z rotation to the angle
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }

        // If the user is using the tool
        if (parent.CurrentCom.useTool && cooldownTimer <= 0f) {

            // Set the tool hitbox to be active
            toolSprite.GetComponent<SpriteRenderer>().color = hitColour;
            toolSprite.GetComponent<BoxCollider2D>().size = Vector2.one;
            // Start the hit frame timer
            hitTimer = hitActive;

        }

        // Run any active timers
        RunTimer();

    }

    void RunTimer() {

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
