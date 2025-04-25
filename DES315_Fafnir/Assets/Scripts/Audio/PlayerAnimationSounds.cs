using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimationSounds : MonoBehaviour // sounds that are attached as events to player's animations
{
    // footsteps
    [SerializeField]
    protected string Step1;

    [SerializeField]
    protected string Step2;

    // armor
    [SerializeField]
    protected string Armor1;

    [SerializeField] 
    protected string Armor2;

    // jump/land
    [SerializeField]
    protected string Jump;

    [SerializeField]
    protected string LandSound;

   

    public void PlaySteps1() // plays on keyframe 6 of walking animation
    {
        Audiomanager.instance.PlayAudio(Step1, 0.25f);
        Audiomanager.instance.PlayAudio(Armor1);
    }

    public void PlaySteps2() // plays on keyframe 12 of walking animation
    {
        Audiomanager.instance.PlayAudio(Step2, 0.25f);
        Audiomanager.instance.PlayAudio(Armor2);
    }

    public void JumpSound() // plays on keyframe 1 of jumping animation
    {
        Audiomanager.instance.PlayAudio(Jump);

    }


    public void MaxHeightEvent() // when player reaches peak of jumping 
    { StartCoroutine(WaitForLanding()); }

    public IEnumerator WaitForLanding() // pauses on keyframe 4 of jumping animation while airborne
    {

        Animator anim = GetComponent<Animator>();
        anim.speed = 0f;

        yield return new WaitUntil(Land);
        Audiomanager.instance.PlayAudio(LandSound);
        anim.speed = 1f;

    }

    public bool Land() // keyframe 7 of jumping animation
    {
        //Audiomanager.instance.PlayAudio(Step2, 0.25f);
        return FindFirstObjectByType<PlayerAI>().Rb.velocityY == 0f; 
    }

    public void Swap() // swaps value of boolean
    { GetComponent<Animator>().SetBool("InAir", false); }

}
