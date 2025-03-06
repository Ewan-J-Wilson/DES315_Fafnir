using UnityEngine;

public class ActivateDrawbridge : ActivateGeneric
{
    
    private ActivateRotatable[] children;

    // Start is called before the first frame update
    void Start()
    {
        isHold = true;
        children = GetComponentsInChildren<ActivateRotatable>();
    }

    // Update is called once per frame
    public override void Update()
    {

        ThresholdTracker();

        foreach (ActivateRotatable child in children)
        { child.SetActive(isActive); }
       
    }
}
