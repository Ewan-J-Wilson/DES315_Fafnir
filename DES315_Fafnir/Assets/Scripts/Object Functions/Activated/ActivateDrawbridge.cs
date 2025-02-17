using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDrawbridge : ActivateGeneric
{
    
    public AnimationCurve Curve;        //Used to change speed of bridge over time
    public Transform[] Trans;           //Array of elements to shift
    float[] AngleBase;                  //Starting angle of object
    public float[] AngleTarget;         //Angle offset target for maximum CurveTime
    float CurveTime = 0;                //Float time between 0 and 1 for Curve
    public float BridgeSpeed;           //How fast to inc/dec curve time
    // Start is called before the first frame update
    void Start()
    {
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
            float angle = AngleBase[x] + Curve.Evaluate(CurveTime) * AngleTarget[x];
            Trans[x].rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
