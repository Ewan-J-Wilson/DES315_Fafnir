using UnityEngine;

public class ActivateGeneric : MonoBehaviour {

    [HideInInspector]
    public int thresholdCount = 0;
    [SerializeField] [Range(1,5)]
    protected int threshold = 1;

    protected bool isHold = false;
    protected bool isActive = false;

    public virtual void Update() {

        ThresholdTracker();

    }

    public void ThresholdTracker() {

        if (thresholdCount >= threshold && !isActive) { 
            isActive = true;
            if (!isHold)
            { DoAction(); } 
        }
        else if (thresholdCount < threshold && isActive) {
            isActive = false;
            if (!isHold)
            { DoAction(); }
        }
        else
        { return; }

    }

    public void SetActive(bool _isActive)
    { isActive = _isActive; }

    protected virtual void DoAction() 
    { Debug.Log("You're not supposed to be in here"); }

}