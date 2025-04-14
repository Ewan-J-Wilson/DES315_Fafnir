using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { 
        SceneManager.sceneLoaded += LoadCamera;
        LoadCamera(gameObject.scene, LoadSceneMode.Single);
    }


    private void LoadCamera(Scene arg0,LoadSceneMode arg1) 
    { GetComponent<Canvas>().worldCamera = FindFirstObjectByType<Camera>(); }


    // Why does this stay on destruction in the first place ;-;
    private void OnDestroy() 
    { SceneManager.sceneLoaded -= LoadCamera; }

}
