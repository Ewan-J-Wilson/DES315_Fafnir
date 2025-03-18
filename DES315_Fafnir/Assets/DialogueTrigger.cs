using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField] private string chapter;
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
            triggered = true;

        }

    }

}
