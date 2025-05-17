using UnityEngine;
using UnityEngine.EventSystems;

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
