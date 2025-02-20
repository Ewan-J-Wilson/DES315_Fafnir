using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class ActivateRotatable : ActivateGeneric {

    public AnimationCurve Curve;        //Used to change speed of bridge over time
    private float AngleBase;            //Starting angle of object
    public float AngleTarget = 90;
    private float CurveTime = 0;
    public float BridgeSpeed;
    private bool isDrawbridge = false;

    void Start()
    {
        isHold = true;
        AngleBase = transform.eulerAngles.z;

        if (transform.parent != null)
        { isDrawbridge = transform.parent.TryGetComponent<ActivateDrawbridge>(out _); }
    }

    public override void Update()
    {

        if (!isDrawbridge)
        { ThresholdTracker(); }

        CurveTime = Mathf.Clamp(CurveTime, 0, 1);
        CurveTime += Time.deltaTime * (isActive ? -BridgeSpeed : BridgeSpeed);

        float angle = AngleBase + Curve.Evaluate(CurveTime) * AngleTarget;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
           
    }

}
