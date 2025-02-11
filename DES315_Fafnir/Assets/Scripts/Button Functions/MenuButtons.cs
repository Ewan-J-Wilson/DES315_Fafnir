using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{

    // Generic condition to change when swapping scenes
    public static bool swapCondition = false;

    // If the menu only appears after a condition has been met
    [SerializeField] private bool conditional = false;
    private EventSystem eventSys;

    private void Awake() { 
        GetComponent<Canvas>().enabled = !conditional; 
        eventSys = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        eventSys.enabled = !conditional;
    }

    public static void SwitchToScene(System.String _scene) { 
        Time.timeScale = 1;
        SceneManager.LoadScene(_scene); 
    }

    public static void SwitchToScene(System.String _scene, bool _condition) { 
        Time.timeScale = 1;

        SceneManager.LoadScene(_scene); 
    }
    
    public void Pause(bool _pause) {
        GetComponent<Canvas>().enabled = _pause;
        eventSys.enabled = _pause;
        Time.timeScale = (_pause ? 0 : 1);
    }

    public static void ExitGame() 
    { Application.Quit(); }

}
