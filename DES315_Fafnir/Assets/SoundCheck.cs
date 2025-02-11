using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCheck : MonoBehaviour
{

    void Awake() {

        

    }

    // Start is called before the first frame update
    void Start()
    {
        Audiomanager.instance.PlayAudio("Minceraft");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
