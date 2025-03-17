using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField] private string chapter;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !DialogueRead.reading) {
            DialogueManager.chapter = chapter;
            DialogueManager.OnDialogueObject();
        }
    }
}
