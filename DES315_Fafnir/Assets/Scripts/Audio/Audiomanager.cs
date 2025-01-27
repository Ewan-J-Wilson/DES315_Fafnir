using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType
{
    MUSIC,
    SFX,
}

[System.Serializable]
public struct AudioInstance
{
    public string Name;                         //Identifier name
    [Range(0f, 1f)] public float Volume;        //Volume of sound
    [Range(0f, 2f)] public float Pitch;         //Pitch of sound
    [Range(0f, 1f)] public float Panning;       //L/R panning
    public bool loop;                           //Loop flag
    public AudioClip clip;                      //Sound to play
    [HideInInspector] public AudioSource src;   //Source to play from
    public AudioType type;                      //Whether it is Music or SFX
}
public class Audiomanager : MonoBehaviour
{
    public AudioMixerGroup AudOut;
    public static Audiomanager instance;
    public AudioInstance[] tracks;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(this);
            return;
        }
        for (int i = 0; i < tracks.Length; i++)
        {
            tracks[i].src = gameObject.AddComponent<AudioSource>();
            tracks[i].src.clip = tracks[i].clip;
            tracks[i].src.volume = tracks[i].Volume;
            tracks[i].src.pitch = tracks[i].Pitch;
            tracks[i].src.panStereo = tracks[i].Panning;
            tracks[i].src.loop = tracks[i].loop;
            tracks[i].src.outputAudioMixerGroup = AudOut;
        }
    }

    public void PlayAudio(string name, float pitch = 0.0f, float pan = 0.5f, float vol = 0.0f)
    {
        AudioInstance aud = Array.Find(tracks, tracks => tracks.Name == name);
        if (pitch != 0.0f) { aud.src.pitch = pitch; }
        if (pan != 0.5f) { aud.src.panStereo = pan; }
        if (vol != 0.0f) { aud.src.volume = vol; }

        aud.src.Play();
    }
}
