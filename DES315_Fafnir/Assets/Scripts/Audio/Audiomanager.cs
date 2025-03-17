using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType
{
    MASTER,
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

[System.Serializable]
public struct LoopTrackInstance
{
    public string[] Name;
}

public class Audiomanager : MonoBehaviour
{
    private const float FADE_SPEED = 0.5f;

    public AudioMixerGroup MusicOut;
    public AudioMixerGroup SFXOut;
    public static Audiomanager instance;
    
    //Audio tracks and fade tracks
    public AudioInstance[] tracks;
    private AudioInstance CurrentTrack;         //Music track to fade in
    private AudioInstance PreviousTrack;        //Music track to fade out

    // SFX 
    public AudioInstance[] sfx;

    public LoopTrackInstance[] LoopTrackList;   //List of music tracks to play in a given level loop
    private float TargetFade;                   //target volume to fade to

    [HideInInspector] 
    public static Dictionary<AudioType, float> volumeLevels = 
        new() { 
            { AudioType.MASTER, 1.0f },
            { AudioType.MUSIC, 0.2f },
            { AudioType.SFX, 1.0f },
        };
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < tracks.Length; i++)
        {
            tracks[i].src = gameObject.AddComponent<AudioSource>();
            tracks[i].src.clip = tracks[i].clip;
            tracks[i].src.volume = tracks[i].Volume;
            tracks[i].src.pitch = tracks[i].Pitch;
            tracks[i].src.panStereo = tracks[i].Panning;
            tracks[i].src.loop = tracks[i].loop;
            tracks[i].src.outputAudioMixerGroup = MusicOut;
        }

        for (int i = 0; i < sfx.Length; i++)
        {
            sfx[i].src = gameObject.AddComponent<AudioSource>();
            sfx[i].src.clip = sfx[i].clip;
            sfx[i].src.volume = sfx[i].Volume;
            sfx[i].src.pitch = sfx[i].Pitch;
            sfx[i].src.panStereo = sfx[i].Panning;
            sfx[i].src.loop = sfx[i].loop;
            sfx[i].src.outputAudioMixerGroup = SFXOut;
        }

    }
    private void Start()
    {
        FadeLoopTracks(0, 0);
    }

    public void Update()
    {
        HandleFade();
    }

    void HandleFade()
    {
        if (PreviousTrack.Name != null)
        {
            if (PreviousTrack.src.volume != 0)
            {
                PreviousTrack.src.volume -= Time.deltaTime * FADE_SPEED;
                if (PreviousTrack.src.volume < 0)
                {
                    PreviousTrack.src.volume = 0;
                }
            }
        }

        if (CurrentTrack.Name != null)
        {
            if (CurrentTrack.src.volume != volumeLevels[CurrentTrack.type])
            {
                CurrentTrack.src.volume += Time.deltaTime * FADE_SPEED;
                if (CurrentTrack.src.volume > volumeLevels[CurrentTrack.type])
                {
                    CurrentTrack.src.volume = volumeLevels[CurrentTrack.type];
                }
            }
        }
    }

    public static void ChangeVolume(float vol, AudioType type)
    { 
        volumeLevels[type] = vol; 
        for (int i = 0; i < instance.tracks.Length; i++) {

            if (instance.tracks[i].type == type) { 
                instance.tracks[i].src.volume = vol;
                if (instance.tracks[i].type != AudioType.MASTER) 
                {instance.tracks[i].src.volume *= volumeLevels[AudioType.MASTER];}
            }
        }
    }

    public void PlayAudio(string name, float vol = 1.0f, float pitch = 0.0f, float pan = 0.5f)
    {
        AudioInstance aud = Array.Find(tracks, tracks => tracks.Name == name);

        if (aud.src == null)
        { aud = Array.Find(sfx, sfx => sfx.Name == name); }

        if (aud.src == null) {
            Debug.Log("Audio Not Found");
            return;
        }

        if (pitch != 0.0f) { aud.src.pitch = pitch; }
        if (pan != 0.5f) { aud.src.panStereo = pan; }
        if (vol != 0.0f)
        {
            aud.src.volume = vol * volumeLevels[aud.type];
            if (aud.type != AudioType.MASTER)
            { aud.src.volume *= volumeLevels[AudioType.MASTER]; }
        }
        else aud.src.volume = 0.0f;
        aud.src.Play();
    }

    //
    //  Notes on the function, alongside some caveats
    //
    //  1, the fade function can only be called for ONE FRAME
    //  2, the function only works in a list that goes forwards, it cannot function going backwards
    //  3, next track MUST be unique
    //
    public void FadeLoopTracks(int loopind, int levelind)
    {
        Debug.Log(levelind + "-" + loopind);

        PreviousTrack = CurrentTrack;
        //if (loopind > 0) 
        //{ PreviousTrack = FindLooptrack(loopind - 1, levelind); }
        //else if (levelind > 0) 
        //{ PreviousTrack = FindLooptrack(loopind, levelind - 1); }
        CurrentTrack = FindLooptrack(loopind, levelind);

        if (CurrentTrack.Name != null) 
        { PlayAudio(CurrentTrack.Name, 0.0f); }

        Debug.Log("CurrentTrack: " + CurrentTrack.Name + "\nPreviousTrack: " + PreviousTrack.Name);
    }
    
    public AudioInstance FindLooptrack(int loopind, int levelind) 
    { return Array.Find(tracks, tracks => tracks.Name == LoopTrackList[levelind].Name[loopind]); }
}
