using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MoveableWall : MonoBehaviour
{
    private enum Direction{Left, Right, Up, Down}

    [SerializeField] private float moveDistance = 1;
    [Tooltip("NOTE: Currently affects the Hold Duration, so adjust that accordingly")]
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private Direction destination = Direction.Right;
    [SerializeField] private bool keepCollision = false;
    [SerializeField] private bool oneShot = true;
    [SerializeField] private bool moveBack = false;
    [Tooltip("NOTE: Currently affected by the Move Speed")]
    [SerializeField] private float holdDuration = 3f;
    private float holdTimer = 0f;
    private bool moved = false;

    private bool used = false;
    private float moveTimer = 0;
    private Vector3 moveDir;

    // Start is called before the first frame update
    void Awake()
    {
     
        // Sets the appropriate direction vector
        moveDir = destination switch {

            Direction.Right => Vector3.right,
            Direction.Left => Vector3.left,
            Direction.Up => Vector3.up,
            Direction.Down => Vector3.down,
            _ => Vector3.zero
        
        };

    }

    // Update is called once per frame
    void Update()
    {

        if (!moveBack) {

            if (used && moveTimer <= 0)
            { used = false; }

            MoveWall(moveDir, ref moveTimer);
            return;

        }

        // Hold the wall/platform in position for the set time
        if (used && moveTimer <= 0f && holdTimer <= 0f) { 
            holdTimer = holdDuration; 
            moved = true;
        }
        else if (used && moveTimer <= 0f)
        { holdTimer -= Time.deltaTime; }

        // Move the wall in the desired direction, or move it back if needed
        if (used && holdTimer <= 0f && moved) { 
            moveTimer = moveDistance; 
            used = false;
        }
        else if (moved && moveTimer > 0)
        { MoveWall(-moveDir, ref moveTimer); }
        else if (!moved && moveTimer > 0)
        { MoveWall(moveDir, ref moveTimer); }

        // If not in use, reset
        if (moved && moveTimer <= 0f && holdTimer <= 0f) 
        { moved = false; }

    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision) {
        
        //Debug.Log(collision.gameObject.name);

        // Checks if the collision is with the Tool's sprite
        // as well as if the collision already occurred
        // and if the object is already currently moving
        if (collision.gameObject.name != "Tool Sprite" || used || moveTimer > 0)
        { return; }
        used = true;
        
        // Disables collision if needed
        GetComponent<Collider2D>().enabled = keepCollision || !oneShot;
        // Keeps the ability to trigger it again with no physical collision
        GetComponent<Collider2D>().isTrigger = !keepCollision && !oneShot;

        // Sets the wall in motion
        moveTimer = moveDistance;

    }

    private void MoveWall(Vector3 _dir, ref float _timer) {

        // Moves the wall to the expected location
        if (_timer > 0) {
            
            transform.Translate(moveSpeed * Time.deltaTime * _dir);
            // NOTE: moveSpeed currently affects holdTimer
            _timer -= Time.deltaTime * moveSpeed;

        }

    }

}
