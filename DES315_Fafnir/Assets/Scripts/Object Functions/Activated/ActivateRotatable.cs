using System;
using UnityEngine;

public class ActivateRotatable : ActivateGeneric {

    [Tooltip("The acceleration curve of this object")]
    public AnimationCurve Curve;        //Used to change speed of bridge over time
    private float AngleBase;            //Starting angle of object
    [Tooltip("The angle (degrees) that this object will move to")]
    [Range(-180,180)]
    public float AngleTarget = 90;
    private float CurveTime = 0;
    [Tooltip("The speed this object will move")]
    public float platformSpeed = 1.5f;
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
        CurveTime += Time.deltaTime * (isActive ? -platformSpeed : platformSpeed);

        float angle = AngleBase + Curve.Evaluate(CurveTime) * AngleTarget;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
           
    }

}
