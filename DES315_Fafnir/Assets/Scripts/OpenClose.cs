using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClose : MonoBehaviour
{
    public Door testDoor;
    public float xScale;
    // Start is called before the first frame update
    void Start()
    {
        xScale = 1.0f;
    }

    // Update is called once per frame
    public void OpenDoor()
    {
        if (testDoor.isActive == true)
        {
            xScale *= 1.0f;
            transform.localScale = new Vector3(xScale, 1.0f, 1.0f);
        }
        

    }
}
