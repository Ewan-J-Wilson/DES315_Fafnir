using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenuButtons : MonoBehaviour
{

    public Canvas menu;
    public Button btnResume, btnReturn;
    public System.String menuScene;
   // public UnityEvent onHover = new UnityEvent();

    // Start is called before the first frame update
    void Start() {

        btnResume.onClick.AddListener(delegate{pause(false);});
        btnReturn.onClick.AddListener(toMenu);

        pause(false);
        
    }

    void Update() {

        if (Input.GetButtonDown("Pause")) 
        { pause(!menu.GetComponent<Canvas>().enabled); }

    }


    void pause(bool _pause) {

        menu.GetComponent<Canvas>().enabled = _pause;
        Time.timeScale = (_pause ? 0 : 1);

    }

    void toMenu() 
    { SceneManager.LoadScene(menuScene); }

}
