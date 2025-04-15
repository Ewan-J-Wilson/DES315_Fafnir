using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    // Generic condition to change when swapping scenes
    public static bool swapCondition = false;
    private static bool doNextScene = false;
    private static string nextScene = "";

    [HideInInspector]
    public static Fade _fade;

    // If the menu only appears after a condition has been met
    [Tooltip("Does this only show up after certain conditions are met?")]
    [SerializeField] private bool conditional = false;

    private EventSystem eventSys;

    private void Awake() { 
        GetComponent<Canvas>().enabled = !conditional; 
        eventSys = FindFirstObjectByType<EventSystem>();
        _fade = GameObject.FindGameObjectWithTag("FadeOut").GetComponent<Fade>();
        eventSys.enabled = !conditional;
    }

    // String for now because AssetReference does not seem to be serialisable as
    // a parameter in an editor function
    public static void SwitchToScene(string _scene) { 
        
        _fade.FadeOut();
        doNextScene = true;
        nextScene = _scene;
        if (DialogueManager.instance != null)
        {
            if (DialogueManager.instance.textBox.isActiveAndEnabled)
            { DialogueManager.instance.textBox.EndDialogue(); }
        }
        Audiomanager.instance.PlayAudio("Select");
        //SceneManager.LoadSceneAsync(_scene);
    }

    public void SwitchToControlsScene(bool _condition) {
        
        swapCondition = _condition;
        _fade.FadeOut();
        doNextScene = true;
        nextScene = "Controls Menu";
        Audiomanager.instance.PlayAudio("MenuConfirm");
        //Time.timeScale = 1;
        //SceneManager.LoadSceneAsync("Controls Menu"); 

    }

    public void ShowHint() {

        if (DialogueRead.reading)
        { return; }
        Pause(false);
        DialogueManager.LoopTrigger("HINT");

    }

    public void Update() {
        
		if (doNextScene && _fade.alpha >= 1f) {
			doNextScene = false;
            Time.timeScale = 1;
			SceneManager.LoadSceneAsync(nextScene);
		}

    }
    
    public void Pause(bool _pause) {

        // Enable the pause
        GetComponent<Canvas>().enabled = _pause;
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
