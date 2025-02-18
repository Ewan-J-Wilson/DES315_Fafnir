using UnityEditor;
using UnityEngine;

public class ActivateMoveable : ActivateGeneric
{
    public AnimationCurve Curve;        //Used to change speed of bridge over time
    public Vector2 TargetPos;           //Position to go to when moving
    public float Speed;                 //Determine how fast the object moves
    Vector2 OriginPos;                  //Original position object was at
    float CurveTimer = 0;               //Timer for curve
    // Start is called before the first frame update
    void Start()
    {
        isHold = true;
        OriginPos = transform.position;
    }

    // Update is called once per frame
    public override void Update()
    {
        ThresholdTracker();

        CurveTimer += Time.deltaTime * (isActive ? Speed : -Speed);
        CurveTimer = Mathf.Clamp(CurveTimer, 0, 1);

        transform.position = Vector3.Lerp(OriginPos, TargetPos, Curve.Evaluate(CurveTimer));
    }
}
