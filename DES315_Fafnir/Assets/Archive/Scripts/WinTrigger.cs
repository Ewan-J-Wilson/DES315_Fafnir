using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{

    // Player overlap flag
    private bool playerOverlap = false;
    // Reference to Win Screen
    [SerializeReference] GameObject winScreen;
    // Win flag
    [NonSerialized] public static bool hasWon;

    // Update is called once per frame
    void Update()
    {

        // If the player is on the exit and W is pressed
        if (playerOverlap && Input.GetButtonDown("Up")) {

            //Debug.Log("Game Won");
            // Set the win flag
            hasWon = true;
            // Show the win screen
            winScreen.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        // If the collision is from the player, update the flag
        if (collision.gameObject.name != "Player")
        { return; }
        //Debug.Log("Collision Entered");
        playerOverlap = true;

    }

    private void OnTriggerExit2D(Collider2D collision) {
        
        // If the collision is from the player, update the flag
         if (collision.gameObject.name != "Player")
         { return; }
         //Debug.Log("Collision Left");
        playerOverlap = false;

    }

}
