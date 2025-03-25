using UnityEngine;

public class SecurityCamera : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision) // destroys clones when player enters camera trigger
    {
        if (!collision.CompareTag("Player")) // ensures that this only occurs when the player is inside the trigger, and not the clones themselves
        {
            return;
        }
        PlayerAI player = collision.gameObject.GetComponent<PlayerAI>();
        player.KillClone(); // calls function from player script 
    }

}
