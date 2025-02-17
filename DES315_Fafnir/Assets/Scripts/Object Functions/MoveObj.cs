using UnityEditor;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    public GameObject StateDeterminer;  //Object to take active state off of
    public AnimationCurve Curve;        //Used to change speed of bridge over time
    public Vector2 TargetPos;           //Position to go to when moving
    public float Speed;                 //Determine how fast the object moves
    Vector2 OriginPos;                  //Original position object was at
    bool IsActive = false;              //Flag for active/inactive state
    float CurveTimer = 0;               //Timer for curve
    // Start is called before the first frame update
    void Start()
    {
        OriginPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        IsActive = !StateDeterminer.activeSelf;

        if (IsActive)
        {
            CurveTimer += Time.deltaTime * Speed;
        }
        else
        {
            CurveTimer -= Time.deltaTime * Speed;
        }

        CurveTimer = Mathf.Clamp01(CurveTimer);

        transform.position = Vector3.Lerp(OriginPos, TargetPos, Curve.Evaluate(CurveTimer));
    }
}
