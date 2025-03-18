using UnityEngine;

public class ActivateGeneric : MonoBehaviour {

    [HideInInspector]
    public int thresholdCount = 0;
    [Tooltip("The amount of triggers needed at once to activate this object")]

    [SerializeField] [Range(1,5)]
    protected int threshold = 1;

    [SerializeField]
    protected string ActivateSound;

    protected bool isHold = false;
    protected bool isActive = false;

    public virtual void Update() {

        ThresholdTracker();

    }

    public void ThresholdTracker() {

        if (thresholdCount >= threshold && !isActive)
        {
            SetActive(true);

            if (!isHold)
            { DoAction(); }
        }
        else if (thresholdCount < threshold && isActive)
        {
            SetActive(false);
            if (!isHold)
            { DoAction(); }
        }
        else
        { return; }

       
    }
    
    public void SetActive(bool _isActive)
    {    
        isActive = _isActive;
        Audiomanager.instance.PlayAudio(_isActive ? ActivateSound : null);
    }

    protected virtual void DoAction()
    { Debug.Log("You're not supposed to be in here"); }
       

}