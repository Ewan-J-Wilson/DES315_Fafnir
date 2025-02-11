using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectControlUI : MonoBehaviour, ISelectHandler
{
    UpdateControlPos controlRef;

    // Get the reference to the group object
    void Start()
    { controlRef = GetComponentInParent<UpdateControlPos>(); }

    // If selected, move the scrollbar accordingly
    public void OnSelect(BaseEventData eventData) 
    { controlRef.ScrollMove(); }

}
