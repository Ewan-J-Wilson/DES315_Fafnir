using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextScale : MonoBehaviour
{

    Slider slider;
    [SerializeField]
    private GameObject testDialogue;

    // Start is called before the first frame update
    void Start()
    {
        
        // Initialise the Slider
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("Text Scale");
        slider.onValueChanged.AddListener(ValueChanged);
    }

    private void ValueChanged(float value)
    { 

        // Save the preference
        PlayerPrefs.SetFloat("Text Scale", value);
        PlayerPrefs.Save();

        // Update the existing text boxes
        if (DialogueManager.instance != null)
        { DialogueManager.instance.SetDialogueSize(); }
        testDialogue.transform.localScale = new Vector3(value * 1.2f, value * 1.2f, 1);
        testDialogue.GetComponentInChildren<TMP_Text>().fontSize = DialogueManager.defaultFontSize * value;


    }
}
