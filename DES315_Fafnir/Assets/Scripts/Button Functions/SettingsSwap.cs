using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsSwap : MonoBehaviour
{

    [SerializeField]
    private GameObject[] MenuList;
    [SerializeField]
    private string primaryMenu;

    // Initialise the menus
    void Start()
    { SwapMenu(primaryMenu); }

    public void SwapMenu(string _newMenu) {

        // Change the only active menu to the selected one
        foreach (GameObject _menu in MenuList) 
        { _menu.SetActive(_menu.name == _newMenu); }

        FindFirstObjectByType<EventSystem>().SetSelectedGameObject(GameObject.FindGameObjectWithTag("Back Button"));
        

    }

    // Update which controls menu to go to
    public void SetControlStyle(bool _b) 
    { MenuButtons.swapCondition = _b; }

}
