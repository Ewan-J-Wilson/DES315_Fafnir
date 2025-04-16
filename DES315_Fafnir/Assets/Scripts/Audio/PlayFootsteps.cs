using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootsteps : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Steps1()
    {
        Audiomanager.instance.PlayAudio("Steppies1");
    }

    public void Steps2()
    {
        Audiomanager.instance.PlayAudio("Steppies2");
    }
}
