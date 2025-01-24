using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenuButtons : MonoBehaviour
{
    // Start is called before the first frame update

    public Button toMain, quit;
    public System.String gameScene;

    private void Start() {
        
        toMain.onClick.AddListener(switchToGame);
        quit.onClick.AddListener(exitGame);

    }


    void switchToGame() 
    { SceneManager.LoadScene(gameScene); }
    
    void exitGame() 
    { Application.Quit(); }

}
