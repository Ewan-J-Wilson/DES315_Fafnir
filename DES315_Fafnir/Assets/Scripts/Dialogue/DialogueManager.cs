using TMPro;
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
    public static bool canSkip = true;
    public static float defaultFontSize = 28;
    
    // On Start
    public void Start() {
        
        // Set the dialogue manager to be a singleton
        if (instance == null) 
        { instance = this; }
        // If one already exists, self implode
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        // Disable the textbox
        textBox.gameObject.SetActive(false);
        icon.SetActive(false);

    }

    // Update the icon for the next dialogue prompt to 
    // the same state as the textbox
    public void Update() 
    { icon.SetActive(textBox.isActiveAndEnabled); }

    // Allows for dialogue to be changed when the loop changes
    public static void LoopTrigger(string _type) {
        chapter = currentLevel + "-" + GameManager.LoopInd + "_" + _type;
        DisablePlayerInput(true);
        instance.ReadDialogue();
    }

    // Allows for dialogue to be enabled through code
    public static void CodedDialogue(string _chapter) {
        chapter = _chapter;
        DisablePlayerInput(true);
        instance.ReadDialogue();
    }

    // Enable/Disable player input
    public static void DisablePlayerInput(bool _disable) {
        PlayerAI.inputRef.SwitchCurrentActionMap(_disable ? "UI" : "Player");
    }

    // If the Next Action button is pressed
    public void NextAction(InputAction.CallbackContext obj) {

        // Check for if the action was just performed and return otherwise
        if (!obj.performed) {
            next = false;
            return; 
        }

        // Flag the Next action
        next = true;
        return;

    }

    // Read the dialogue and thus display it
    public void ReadDialogue() {
        instance.textBox.gameObject.SetActive(true);
        SetDialogueSize();
        textBox.ReadStart();
    }

    // Update the size of the dialogue box and the font
    public void SetDialogueSize() {

        if (!instance.textBox.gameObject.activeSelf)
        { return; }

        instance.textBox.gameObject.transform.localScale = new Vector3(PlayerPrefs.GetFloat("Text Scale") * 1.2f, PlayerPrefs.GetFloat("Text Scale") * 1.2f, 1);
        instance.GetComponentInChildren<TMP_Text>().fontSize = defaultFontSize * PlayerPrefs.GetFloat("Text Scale");

    }


}