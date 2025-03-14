using UnityEngine;

public class SignalManager : MonoBehaviour
{

    public static int currentLevel = 0;
    public static SignalManager instance;
    public static string chapter;
    public DialogueRead textBox;

    public void Start() {
        
        instance = this;

        DontDestroyOnLoad(gameObject);

    }

    public static void OnLoopChange() {
    
        chapter = currentLevel + "-" + GameManager.LoopInd + "_START";
        instance.ReadDialogue();
    
    }
    
    public static void OnDialogueObject() {
        Debug.Log(chapter);
        instance.ReadDialogue();
    
    }

    public void ReadDialogue() {

        textBox.ReadStart();

    }


}