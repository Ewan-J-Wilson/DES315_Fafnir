using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    public static int currentLevel = 0;
    public static DialogueManager instance;
    public static string chapter;
    public DialogueRead textBox;
    public GameObject icon;
    public static bool next = false;
    public static bool autoscroll = false;
    public static float autoscrollLength = 3f;
    

    public void Start() {
        

        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //LoadCamera(gameObject.scene, LoadSceneMode.Single);
        //SceneManager.sceneLoaded += LoadCamera;

        textBox.gameObject.SetActive(false);
        icon.SetActive(false);

    }

    public void Update() {

        icon.SetActive(textBox.isActiveAndEnabled);

    }

    //public void LoadCamera(Scene scene, LoadSceneMode mode)
    //{ GetComponent<Canvas>().worldCamera = FindFirstObjectByType<Camera>(); }


    public static void OnLoopChange() {
    
        chapter = currentLevel + "-" + GameManager.LoopInd + "_START";
        instance.ReadDialogue();
    
    }
    
    public static void OnDialogueObject() {
        Debug.Log(chapter);
        instance.ReadDialogue();
    
    }

    public static void CodedDialogue(string _chapter) {

        chapter = _chapter;
        instance.ReadDialogue();

    }

    public void NextAction(InputAction.CallbackContext obj) {

        if (!obj.performed) {
            next = false;
            return; 
        }

        next = true;
        return;

    }

    public static void DisablePlayerInput(bool _disable) {
        
        PlayerInput input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        input.SwitchCurrentActionMap(_disable ? "UI" : "Player");
        
    }

    public void ReadDialogue() {
        
        instance.textBox.gameObject.SetActive(true);
        textBox.ReadStart();

    }


}