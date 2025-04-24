using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    // Condition to show which controls settings menu to swap to
    public static bool swapCondition = false;

    private static bool doNextScene = false;
    private static string nextScene = "";

    [HideInInspector]
    public static Fade _fade;

    // If the menu only appears after a condition has been met
    //[Tooltip("Does this only show up after certain conditions are met?")]
    //[SerializeField] private bool conditional = false;

    private EventSystem eventSys;

    private void Awake() { 
        //if (name != Pause)
        //GetComponent<Canvas>().enabled = !conditional; 
        eventSys = FindFirstObjectByType<EventSystem>();
        _fade = GameObject.FindGameObjectWithTag("FadeOut").GetComponent<Fade>();
        //eventSys.enabled = !conditional;
    }

    // Loads a new scene
    public static void SwitchToScene(string _scene) { 
        
        _fade.FadeOut();
        doNextScene = true;
        nextScene = _scene;
        if (DialogueManager.instance != null)
        {
            if (DialogueManager.instance.textBox.isActiveAndEnabled)
            { DialogueManager.ForceQuitDialogue(); }
        }
        Audiomanager.instance.PlayAudio("Select");
    }

    /*
     Now defunct function due to the controls settings now being a prefab
    */
    public void SwitchToControlsScene(bool _condition) {
        
        swapCondition = _condition;
        _fade.FadeOut();
        doNextScene = true;
        nextScene = "Controls Menu";
        Audiomanager.instance.PlayAudio("Select");

    }

    // Show the relevant level hint
    public void ShowHint() {

        Audiomanager.instance.PlayAudio("Select");

        if (DialogueRead.reading)
        { return; }
        Pause(false);
        DialogueManager.LoopTrigger("HINT");

    }

    public void Update() {
        
        // If the fade to black has finished, load the next level
		if (doNextScene && Fade.alpha >= 1f) {
			doNextScene = false;
            Time.timeScale = 1;
			SceneManager.LoadSceneAsync(nextScene);
		}

    }
    
    public void Pause(bool _pause) {

        // Enable the pause
        //GetComponent<Canvas>().enabled = _pause;
        FindFirstObjectByType<SettingsSwap>().SwapMenu(_pause ? "Pause Menu" : "NULL");
        eventSys.enabled = _pause;
        // Stops Time
        Time.timeScale = (_pause ? 0 : 1);

        // Swaps to the UI action map if the game is paused
 
        PlayerInput actions = GetComponentInParent<PlayerInput>();
        if (_pause) 
        { actions.SwitchCurrentActionMap("UI"); }
        else 
        { actions.SwitchCurrentActionMap("Player"); }

    }

    public static void ExitGame() 
    { Application.Quit(); }

}
