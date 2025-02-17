using UnityEngine;

public class ActivateGeneric : MonoBehaviour {

    [HideInInspector]
    public int thresholdCount = 0;
    [SerializeField] [Range(1,5)]
    protected int threshold = 1;

    
    protected bool isActive = false;

    public void Update() {

        if (thresholdCount >= threshold && !isActive) { 
            isActive = true;
            DoAction(); 
        }
        else if (thresholdCount < threshold && isActive) {
            isActive = false;
            DoAction();
        }
        else
        { return; }

    }

    protected virtual void DoAction() 
    { Debug.Log("You're not supposed to be in here"); }

}