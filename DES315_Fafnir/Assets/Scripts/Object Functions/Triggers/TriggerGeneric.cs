using UnityEngine;

public class TriggerGeneric : MonoBehaviour {

    [SerializeReference]
    [Tooltip("List of items that this object triggers")]
    protected ActivateGeneric[] triggerList;

    [HideInInspector]
    public bool isActive = false;
    protected bool canSelect = false;
    [HideInInspector]
    public bool selected = false;

    public void OnTrigger() {
        Debug.Log("pls");
        foreach (ActivateGeneric toActivate in triggerList)
        { toActivate.thresholdCount++; }
    }

    public void OnExit() {
        Debug.Log("why");
        foreach (ActivateGeneric toActivate in triggerList)
        { toActivate.thresholdCount--; }
    }

    public void Reset() {

        isActive = false;
        foreach (ActivateGeneric active in triggerList)
        { active.thresholdCount = 0; }

    }

}