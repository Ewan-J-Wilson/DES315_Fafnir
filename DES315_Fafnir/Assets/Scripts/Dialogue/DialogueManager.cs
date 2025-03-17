using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    public static int currentLevel = 0;
    public static DialogueManager instance;
    public static string chapter;
    public DialogueRead textBox;

    public void Start() {
        
        LoadCamera(gameObject.scene, LoadSceneMode.Single);
        SceneManager.sceneLoaded += LoadCamera;

        instance = this;
        textBox.gameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
        

    }

    public void LoadCamera(Scene scene, LoadSceneMode mode)
    { GetComponent<Canvas>().worldCamera = FindFirstObjectByType<Camera>(); }


    public static void OnLoopChange() {
    
        chapter = currentLevel + "-" + GameManager.LoopInd + "_START";
        instance.ReadDialogue();
    
    }
    
    public static void OnDialogueObject() {
        Debug.Log(chapter);
        instance.ReadDialogue();
    
    }

    public void ReadDialogue() {

        instance.textBox.gameObject.SetActive(true);
        textBox.ReadStart();

    }


}