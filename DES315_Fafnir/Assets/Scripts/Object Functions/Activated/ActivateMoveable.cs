using UnityEngine;

public class ActivateMoveable : ActivateGeneric
{

    [Tooltip("The acceleration curve of this object")]
    public AnimationCurve Curve;        //Used to change speed of bridge over time
    [Tooltip("The distance this object will move")]
    public Vector2 TargetPos;           //Position to go to when moving

    [Tooltip("The speed this object will move")]
    public float Speed;                 //Determine how fast the object moves
    private Vector2 OriginPos;                  //Original position object was at
    private float CurveTimer = 0;               //Timer for curve
    // Start is called before the first frame update
    void Start()
    {
        isHold = true;
        OriginPos = transform.position;
    }

    // Update is called once per frame
    public override void Update() // updates platform's position with the passage of time 
    {
        ThresholdTracker();

        CurveTimer += Time.deltaTime * (isActive ? Speed : -Speed);
        CurveTimer = Mathf.Clamp(CurveTimer, 0, 1);

        transform.position = Vector3.Lerp(OriginPos, TargetPos, Curve.Evaluate(CurveTimer));
    }
}
