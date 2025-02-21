using UnityEngine;

public class TriggerGeneric : MonoBehaviour {

    [SerializeReference]
    protected ActivateGeneric[] triggerList;

    protected bool canSelect = false;
    [HideInInspector]
    public bool selected = false;

    
    protected void OnTrigger() { 
        foreach (ActivateGeneric toActivate in triggerList)
        { toActivate.thresholdCount++; }
    }

    protected void OnExit() { 
        foreach (ActivateGeneric toActivate in triggerList)
        { toActivate.thresholdCount--; }
    }

}