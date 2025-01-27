using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MoveableWall : MonoBehaviour
{
    private enum Direction{Left, Right, Up, Down}

    [SerializeField] float moveDistance = 1;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] Direction destination = Direction.Right;
    [SerializeField] bool keepCollision = false;
    [SerializeField] bool oneShot = true;
    
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
     
        // Moves the wall to the expected location
        if (moveTimer > 0) {
            
            transform.Translate(moveDir * Time.deltaTime * moveSpeed);
            moveTimer -= Time.deltaTime * moveSpeed;

        }

    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision) {
        
        //Debug.Log(collision.gameObject.name);

        // Checks if the collision is with the Tool's sprite
        // as well as if the collision already occurred
        // and if the object is already currently moving
        if (collision.gameObject.name != "Tool Sprite" || (oneShot && used) || moveTimer > 0)
        { return; }
        used = true;
        
        // Disables collision if needed
        GetComponent<Collider2D>().enabled = keepCollision || !oneShot;
        // Keeps the ability to trigger it again with no physical collision
        GetComponent<Collider2D>().isTrigger = !keepCollision && !oneShot;

        // Sets the wall in motion
        moveTimer = moveDistance;

    }

}
