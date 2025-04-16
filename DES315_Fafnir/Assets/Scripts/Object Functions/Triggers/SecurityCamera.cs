using Unity.VisualScripting;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField]
    private bool triggerDialogue = false;
    [SerializeField]
    private string chapter;

    private bool triggered = false;

    [Tooltip("Used for dialogue that only triggers once in the entire game")]
    [SerializeField]
    private bool onlyFirstTime = false;
    public static bool firstTimeTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision) // destroys clones when player enters camera trigger
    {
        if (!collision.CompareTag("Player")) // ensures that this only occurs when the player is inside the trigger, and not the clones themselves
        {
            return;
        }

        PlayerAI player = collision.gameObject.GetComponent<PlayerAI>();

        if (player.CloneNo == 0)
        { return; }

        player.KillClone(); // calls function from player script 

        // If this dialogue is not supposed to trigger, break early
        if (onlyFirstTime && firstTimeTriggered)
        { return; }

        // Start the dialogue
        if (triggerDialogue && !triggered) { 
            DialogueManager.CodedDialogue(chapter); 

            // Flag the dialogue as played
            triggered = true;

            if (onlyFirstTime)
            { firstTimeTriggered = true; }

        }

    }

}
