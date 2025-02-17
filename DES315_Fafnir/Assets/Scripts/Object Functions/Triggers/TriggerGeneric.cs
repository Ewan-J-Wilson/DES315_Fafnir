using UnityEngine;

public class TriggerGeneric : MonoBehaviour {

    [SerializeReference]
    protected ActivateGeneric[] triggerList;

    
    protected void OnTrigger() { 
        Debug.Log("Triggered");
        foreach (ActivateGeneric toActivate in triggerList)
        { toActivate.thresholdCount++; }
    }

    protected void OnExit() { 
        Debug.Log("Exited");
        foreach (ActivateGeneric toActivate in triggerList)
        { toActivate.thresholdCount--; }
    }

}