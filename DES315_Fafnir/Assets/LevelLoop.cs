using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LevelLoop : MonoBehaviour
{

    // Get the player's transform component
    [SerializeReference] Transform player;
    // Whether to show the debug sprite or not
    [SerializeField] bool debugSprite = false;

    private void Awake() 
    { GetComponent<SpriteRenderer>().enabled = debugSprite; }

    // Update is called once per frame
    void Update()
    {
        
        // Wrap round from right to left
        if (player.position.x > (transform.position.x + transform.localScale.x/2))
        { player.position =  new (-transform.localScale.x/2, player.position.y, player.position.z);}
        
        // Wrap round from left to right
        if (player.position.x < (transform.position.x - transform.localScale.x/2))
        { player.position =  new (transform.localScale.x/2, player.position.y, player.position.z);}

    }
}
