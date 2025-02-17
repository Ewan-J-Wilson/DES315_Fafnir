using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{

    // Generic condition to change when swapping scenes
    public static bool swapCondition = false;

    // If the menu only appears after a condition has been met
    [SerializeField] private bool conditional = false;
    [SerializeField] private EventSystem eventSys;

    private void Start() { 
        GetComponent<Canvas>().enabled = !conditional; 
        //eventSys = FindFirstObjectByType<EventSystem>();
        eventSys.enabled = !conditional;
    }

    public static void SwitchToScene(System.String _scene) { 
        Time.timeScale = 1;
        SceneManager.LoadScene(_scene); 
    }

    public void SwitchToControlsScene(bool _condition) {

        swapCondition = _condition;

        Time.timeScale = 1;
        SceneManager.LoadScene("Controls Menu"); 

    }

    public static void SwitchToScene(System.String _scene, bool _condition) { 
        Time.timeScale = 1;

        SceneManager.LoadScene(_scene); 
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
