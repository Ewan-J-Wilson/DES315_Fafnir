using System;
using System.Collections.Generic;
using UnityEngine;

public class Drawbridge : MonoBehaviour
{
    public OpenObject OnSwitch;         //Uses open bool to determine state
    public AnimationCurve Curve;        //Used to change speed of bridge over time
    public Transform[] Trans;           //Array of elements to shift
    float[] AngleBase;                  //Starting angle of object
    public float[] AngleTarget;         //Angle offset target for maximum CurveTime
    public bool IsUp = true;            //Flag for determining the state of the drawbridge
    float CurveTime = 0;                //Float time between 0 and 1 for Curve
    public float BridgeSpeed;           //How fast to inc/dec curve time
    // Start is called before the first frame update
    void Start()
    {
        Array.Resize(ref AngleBase, Trans.Length);
        for (int i = 0; i < Trans.Length; i++)
        {
            AngleBase[i] = Trans[i].rotation.eulerAngles.z;
        }
    }

    // Update is called once per frame
    void Update()
    {
        IsUp = !OnSwitch.open;
        CurveTime = Mathf.Clamp(CurveTime, 0, 1);
        if (IsUp)
        {
            CurveTime += Time.deltaTime * BridgeSpeed;
        }
        else
        {
            CurveTime -= Time.deltaTime * BridgeSpeed;
        }

        for (int x = 0; x < Trans.Length; x++)
        {
            Trans[x].rotation = Quaternion.Euler(Trans[x].rotation.eulerAngles.x, Trans[x].rotation.eulerAngles.y, AngleBase[x] + Curve.Evaluate(CurveTime) * AngleTarget[x]);
        }
    }
}
