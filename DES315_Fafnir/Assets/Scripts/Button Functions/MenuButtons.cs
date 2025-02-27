using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

public class MenuButtons : MonoBehaviour
{

    // Generic condition to change when swapping scenes
    public static bool swapCondition = false;

    // If the menu only appears after a condition has been met
    [Tooltip("Does this only show up after certain conditions are met?")]
    [SerializeField] private bool conditional = false;

    private EventSystem eventSys;

    private void Start() { 
        GetComponent<Canvas>().enabled = !conditional; 
        eventSys = FindFirstObjectByType<EventSystem>();
        eventSys.enabled = !conditional;
    }

    // String for now because AssetReference does not seem to be serialisable as
    // a parameter in an editor function
    public static void SwitchToScene(string _scene) { 
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(_scene);
    }

    public void SwitchToControlsScene(bool _condition) {

        swapCondition = _condition;

        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("Controls Menu"); 

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
