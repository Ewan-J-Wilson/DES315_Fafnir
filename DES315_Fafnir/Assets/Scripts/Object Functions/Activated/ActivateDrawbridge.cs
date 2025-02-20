using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDrawbridge : ActivateGeneric
{
    
    public AnimationCurve Curve;        //Used to change speed of bridge over time
    private Transform[] Trans;           //Array of elements to shift
    float[] AngleBase;                  //Starting angle of object
    private float[] AngleTarget = {0, -90, 0, 90, 0};         //Angle offset target for maximum CurveTime
    float CurveTime = 0;                //Float time between 0 and 1 for Curve
    public float BridgeSpeed;           //How fast to inc/dec curve time
    // Start is called before the first frame update
    void Start()
    {
        
        Trans = GetComponentsInChildren<Transform>();
        isHold = true;
        Array.Resize(ref AngleBase, Trans.Length);
        for (int i = 0; i < Trans.Length; i++)
        {
            AngleBase[i] = Trans[i].eulerAngles.z;
        }
    }

    // Update is called once per frame
    public override void Update()
    {

        ThresholdTracker();

        CurveTime = Mathf.Clamp(CurveTime, 0, 1);
        CurveTime += Time.deltaTime * (isActive ? -BridgeSpeed : BridgeSpeed);

        for (int x = 0; x < Trans.Length; x++)
        {
            if (Trans[x].parent == transform) {
                float angle = AngleBase[x] + Curve.Evaluate(CurveTime) * AngleTarget[x];
                Trans[x].rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }
}
