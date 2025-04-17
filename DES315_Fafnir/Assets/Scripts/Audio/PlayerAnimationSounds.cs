using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used for sounds that are attached as events to player's walking animation
public class PlayerAnimationSounds : MonoBehaviour
{
    // footsteps
    [SerializeField]
    protected string Step1;

    [SerializeField]
    protected string Step2;

    [SerializeField]
    protected string Armor1;

    [SerializeField] 
    protected string Armor2;
    
    public void PlaySteps1() // plays on keyframe 6
    {
        Audiomanager.instance.PlayAudio(Step1, 0.25f, 1.0f, 0.5f);
        Audiomanager.instance.PlayAudio(Armor1);
    }

    public void PlaySteps2() // plays on keyframe 12
    {
        Audiomanager.instance.PlayAudio(Step2, 0.25f, 1.0f, 0.5f);
        Audiomanager.instance.PlayAudio(Armor2);
    }
}
