using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainAnimate : MonoBehaviour
{

    private float distance;
    [Tooltip("Speed of the Rain")]
    [SerializeField]
    private float speed;

    // Sets the bounds to loop the sprite between
    void Start()
    { distance = GetComponent<SpriteRenderer>().bounds.size.y / 3; }

    // Update is called once per frame
    void Update()
    {
        // Update the sprite position
        // Looping it when needed
        if ((transform.position -= new Vector3(0f, speed * Time.deltaTime, 0)).y <= -distance) 
        { transform.position += new Vector3(0f, distance*2); }

    }
}
