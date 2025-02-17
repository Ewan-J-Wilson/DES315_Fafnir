using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapControlBinds : MonoBehaviour
{
    private void Awake() {

        //Debug.Log(GameObject.FindGameObjectWithTag("Keyboard").name);
        //Debug.Log(GameObject.FindGameObjectWithTag("Controller").name);

        foreach (GameObject keyboard in GameObject.FindGameObjectsWithTag("Keyboard"))
        { keyboard.SetActive(!MenuButtons.swapCondition); }

        foreach (GameObject gamepad in GameObject.FindGameObjectsWithTag("Controller"))
        { gamepad.SetActive(MenuButtons.swapCondition); }

    }

}
