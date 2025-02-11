using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeReference] private GameObject security; 
    [SerializeReference] PlayerAI player;
    private bool cameraOn = true; // keep this boolean 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) // destroys clones when player enters camera trigger
    {
        if (collision.tag != "Player") // ensures that this only occurs when the player is inside the trigger, and not the clones themselves
        {
            return;
        }
        player.KillClone(); // calls function from player script 
    }
    // Update is called once per frame
    void Update()
    {
     

    }
}
