using UnityEngine;

public class TriggerGeneric : MonoBehaviour {

    [SerializeReference]
    [Tooltip("List of items that this object triggers")]
    protected ActivateGeneric[] triggerList;

    [SerializeField]
    protected string TriggerOnSound;

    [SerializeField] 
    protected string TriggerOffSound;

    [HideInInspector]
    public bool isActive = false;
    protected bool canSelect = false;
    [HideInInspector]
    public bool selected = false;

    // When the object has been activated
    public void OnTrigger() {
        Audiomanager.instance.PlayAudio(TriggerOnSound);
        foreach (ActivateGeneric toActivate in triggerList)
        { toActivate.thresholdCount++; }
    }

    // When the object has been deactivated
    public void OnExit() {
        Audiomanager.instance.PlayAudio(TriggerOffSound);
        foreach (ActivateGeneric toActivate in triggerList) { 
            if (toActivate.thresholdCount > 0)
            { toActivate.thresholdCount--; }
        }
    }

    public void Reset() {

        isActive = false;
        foreach (ActivateGeneric active in triggerList)
        { active.thresholdCount = 0; }

    }

}