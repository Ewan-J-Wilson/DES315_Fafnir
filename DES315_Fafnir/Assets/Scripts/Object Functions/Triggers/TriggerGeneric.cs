using UnityEngine;

public class TriggerGeneric : MonoBehaviour {

    [SerializeReference]
    protected ActivateGeneric[] triggerList;


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

}