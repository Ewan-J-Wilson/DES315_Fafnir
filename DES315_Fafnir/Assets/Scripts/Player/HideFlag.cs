using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HideFlag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    { GetComponent<SpriteRenderer>().enabled = false; }

}
