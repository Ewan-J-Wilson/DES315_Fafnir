using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwapControlBinds : MonoBehaviour
{

    [SerializeField]
    private GameObject[] Panels = { };


    private void OnEnable() {

        // Update the active control scheme settings
        foreach (GameObject obj in Panels)
        { 
            if (obj.CompareTag("Keyboard"))
            { obj.SetActive(!MenuButtons.swapCondition); }

            if (obj.CompareTag("Controller"))
            { obj.SetActive(MenuButtons.swapCondition); }

        }

        FindFirstObjectByType<EventSystem>().SetSelectedGameObject(GameObject.FindGameObjectWithTag("Back Button"));
        //Debug.Log(FindFirstObjectByType<EventSystem>().currentSelectedGameObject);

    }

}
