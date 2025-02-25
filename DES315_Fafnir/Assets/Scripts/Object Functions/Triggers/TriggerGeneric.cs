using UnityEngine;

public class TriggerGeneric : MonoBehaviour {

    [SerializeReference]
    protected ActivateGeneric[] triggerList;
    protected bool isActive = false;
    
    protected void OnTrigger() { 
        foreach (ActivateGeneric toActivate in triggerList)
        { toActivate.thresholdCount++; }
    }

    protected void OnExit() { 
        foreach (ActivateGeneric toActivate in triggerList)
        { toActivate.thresholdCount--; }
    }

    public void Reset() {

        isActive = false;
        foreach (ActivateGeneric active in triggerList)
        { active.thresholdCount = 0; }

    }

}