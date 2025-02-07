using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{

    // If the menu only appears after a condition has been met
    [SerializeField] private bool conditional = false;

    private void Awake()
    { GetComponent<Canvas>().enabled = !conditional; }

    public static void SwitchToScene(System.String _scene) { 
        Time.timeScale = 1;
        SceneManager.LoadScene(_scene); 
    }
    
    public void Pause(bool _pause) {
        GetComponent<Canvas>().enabled = _pause;
        Time.timeScale = (_pause ? 0 : 1);
    }

    public static void ExitGame() 
    { Application.Quit(); }

}
