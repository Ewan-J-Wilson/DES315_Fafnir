using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [Tooltip("Which section of dialogue to trigger")]
    [SerializeField] 
    private string chapter;

    [Tooltip("If enabled, the dialogue trigger can only be activated once")] 
    [SerializeField] 
    private bool triggerOnce = true;

    private bool triggered = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !DialogueRead.reading) {
            DialogueManager.chapter = chapter;
            DialogueManager.OnDialogueObject();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.CompareTag("Player") && !DialogueRead.reading && !triggered) {

            DialogueManager.chapter = chapter;
            DialogueManager.OnDialogueObject();
            if (triggerOnce)
            { triggered = true; }

        }

    }

}
