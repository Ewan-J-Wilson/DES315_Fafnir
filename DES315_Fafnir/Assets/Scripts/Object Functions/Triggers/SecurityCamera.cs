using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeReference] private GameObject security; 
    [SerializeReference] PlayerAI player;

    // cameraOn is commented out so the editor shuts up about a presently unused boolean
    // but this boolean is planned to be used
    //private bool cameraOn = true;


    private void OnTriggerEnter2D(Collider2D collision) // destroys clones when player enters camera trigger
    {
        if (!collision.CompareTag("Player")) // ensures that this only occurs when the player is inside the trigger, and not the clones themselves
        {
            return;
        }
        player.KillClone(); // calls function from player script 
    }

}
