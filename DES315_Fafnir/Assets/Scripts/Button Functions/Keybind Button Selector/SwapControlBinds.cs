using Unity.VisualScripting;
using UnityEngine;

public class SwapControlBinds : MonoBehaviour
{

    [SerializeField]
    private GameObject[] Panels = { };

    private void Awake() {

        //Debug.Log(GameObject.FindGameObjectWithTag("Keyboard").name);
        //Debug.Log(GameObject.FindGameObjectWithTag("Controller").name);

    }

    private void OnEnable() {

        foreach (GameObject obj in Panels)
        { 
            if (obj.CompareTag("Keyboard"))
            { obj.SetActive(!MenuButtons.swapCondition); }

            if (obj.CompareTag("Controller"))
            { obj.SetActive(MenuButtons.swapCondition); }
            
        }

    }

}
