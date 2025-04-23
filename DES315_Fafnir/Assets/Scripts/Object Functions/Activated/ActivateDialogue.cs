using System;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateDialogue : ActivateGeneric
{

    [Tooltip("Which section of dialogue to trigger")]
    [SerializeField] 
    private string chapter; 
    [Tooltip("Whether this object is activated on Activation or Deactivation")]
    [SerializeField]
    private bool onActivate = true; 

    [Tooltip("If enabled, the dialogue trigger can only be activated once")] 
    [SerializeField] 
    private bool triggerOnce = true; 

    private bool triggered = false;

    override protected void DoAction()
    {

        if ((onActivate ? isActive : !isActive) && !triggered) {

            DialogueManager.CodedDialogue(chapter);
            if (triggerOnce)
            { triggered = true; }

        }

    }


}
