using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class SettingsSwap : MonoBehaviour
{

    [SerializeField]
    private GameObject[] MenuList;
    [SerializeField]
    private string primaryMenu;

    // Start is called before the first frame update
    void Start()
    {
        
        // Initialise the menus
        swapMenu(primaryMenu);

    }

    public void swapMenu(string _newMenu) {

        // Change the only active menu to the selected one
        foreach (GameObject _menu in MenuList) 
        { _menu.SetActive(_menu.name == _newMenu); }

    }

    // Update which controls menu to go to
    public void SetControlStyle(bool _b) 
    { MenuButtons.swapCondition = _b; }

}
