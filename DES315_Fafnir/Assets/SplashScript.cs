using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices.WindowsRuntime;

public class SplashScript : MonoBehaviour
{

    [SerializeField]
    float timer = 3f;
    float cooldown = 0f;
    bool doNextScene = false;

    void Start() {
        
        FindFirstObjectByType<Fade>().FadeIn(gameObject.scene, LoadSceneMode.Single);

    }

   public void SkipAction(InputAction.CallbackContext obj) {
        
        if (!obj.performed)
        {return;}
        
        cooldown = timer;

   }
   
   
    void Update() {
    
        if (FindFirstObjectByType<Fade>().IsFading() && cooldown == 0)
        { return; }

        if ((cooldown += Time.deltaTime) >= timer && Fade.alpha <= 0)
        { FindFirstObjectByType<Fade>().FadeOut(); }

        Debug.Log(cooldown);

        if (Fade.alpha >= 1 && !doNextScene) {
            doNextScene = true;
            SceneManager.LoadScene("Main Menu");
        }
    
    }

}
