using UnityEngine;

public class ActivateGeneric : MonoBehaviour {

    [HideInInspector]
    public int thresholdCount = 0; // number of objects that need to be activated to satisfy a given condition
    [Tooltip("The amount of triggers needed at once to activate this object")]

    [SerializeField] [Range(1,5)]
    protected int threshold = 1; // range for threshold

    [SerializeField]
    protected string ActivateSound; // enables sound clip to be played when activated

    protected bool isHold = false; // default values 
    protected bool isActive = false;

    public virtual void Update() {
        // updates tracker accordingly
        ThresholdTracker();

    }

    public void ThresholdTracker() {

        if (thresholdCount >= threshold && !isActive) // sets to true if threshold condition is satisfied
        {
            SetActive(true);

            if (!isHold)
            { DoAction(); }
        }
        else if (thresholdCount < threshold && isActive) // resets to false if threshold goes under the satisfactory value 
        {
            SetActive(false);
            if (!isHold)
            { DoAction(); }
        }
        else
        { return; }

       
    }
    
    public void SetActive(bool _isActive) // function for changing boolean value if activated
    {    
        isActive = _isActive;

        if (isActive)
        // plays relevant sound 
        { Audiomanager.instance.PlayAudio(ActivateSound); } 
    }

    // Default DoAction function
    // Should never be used
    protected virtual void DoAction()
    { Debug.Log("You're not supposed to be in here"); }
       

}