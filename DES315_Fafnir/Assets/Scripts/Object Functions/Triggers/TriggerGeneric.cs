using UnityEngine;

public class TriggerGeneric : MonoBehaviour {

    [SerializeReference]
    protected ActivateGeneric[] triggerList;

    
    protected void OnTrigger() { 
        foreach (ActivateGeneric toActivate in triggerList)
        { toActivate.thresholdCount++; }
    }

    protected void OnExit() { 
        foreach (ActivateGeneric toActivate in triggerList)
        { toActivate.thresholdCount--; }
    }

}