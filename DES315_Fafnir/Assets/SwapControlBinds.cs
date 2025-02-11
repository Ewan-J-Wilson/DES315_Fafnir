using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapControlBinds : MonoBehaviour
{

    void Awake() {

        GameObject.FindGameObjectWithTag("Keyboard").SetActive(!MenuButtons.swapCondition);
        GameObject.FindGameObjectWithTag("Controller").SetActive(MenuButtons.swapCondition);

    }

}
