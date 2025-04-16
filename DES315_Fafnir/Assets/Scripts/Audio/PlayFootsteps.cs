using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootsteps : MonoBehaviour
{

    // used for syncing footsteps with walking animation by attaching the functions to keyframes as events 
    public void Steps1() // plays on keyframe 6
    {
        Audiomanager.instance.PlayAudio("Steppies1");
    }

    public void Steps2() // plays on keyframe 12
    {
        Audiomanager.instance.PlayAudio("Steppies2");
    }
}
