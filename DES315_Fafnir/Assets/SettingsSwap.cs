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
        
        swapMenu(primaryMenu);

    }

    public void swapMenu(string _newMenu) {

        foreach (GameObject _menu in MenuList) {

            _menu.SetActive(_menu.name == _newMenu);

        }

    }

    public void SetControlStyle(bool _b) {

        MenuButtons.swapCondition = _b;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
