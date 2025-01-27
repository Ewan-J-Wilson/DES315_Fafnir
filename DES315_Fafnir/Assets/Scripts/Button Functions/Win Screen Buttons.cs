using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreenButtons : MonoBehaviour
{

    [SerializeField] private Button btnReturn;
    [SerializeField] private System.String menuScene;

    // Start is called before the first frame update
    void Start()
    {

        GetComponent<Canvas>().enabled = false;
        btnReturn.onClick.AddListener(toMenu);
        
    }

    void toMenu() { 

        SceneManager.LoadScene(menuScene);
        Time.timeScale = 1;
        // Disable the win trigger
        WinTrigger.hasWon = false;
    }


}
