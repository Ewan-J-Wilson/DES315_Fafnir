using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAnimate : MonoBehaviour
{

    private float distance;
    [Tooltip("Speed of the Water")]
    [SerializeField]
    private float speed = 3f;

    // Sets the bounds to loop the sprite between
    void Start()
    { distance = GetComponent<SpriteRenderer>().bounds.size.x / 3; }

    // Update is called once per frame
    void Update()
    {
        // Update the sprite position
        // Looping it when needed
        if ((transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f)).x <= -distance) 
        { transform.position += new Vector3(distance*2, 0f); }

    }
}
