using System;
using System.Collections.Generic;
using System.Linq;
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
    public float LoopPoint;                     //Point in time values to set track position to
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
    private const float SAMPLERATE = 48000.0f;
    private const float FADE_SPEED = 0.33f;

    public AudioMixerGroup AudOut;
    public static Audiomanager instance;

    private float CurrentTrackTimer = 0;
    private float PreviousTrackTimer = 0;

    //Audio tracks and fade tracks
    public AudioInstance[] tracks;
    public static AudioInstance[] tracksStatic = {};
    public static AudioInstance CurrentTrack;         //Music track to fade in
    public static AudioInstance PreviousTrack;        //Music track to fade out
    public static AudioInstance NullInst;

    public LoopTrackInstance[] LoopTrackList;   //List of music tracks to play in a given level loop

    [HideInInspector] 
    public static Dictionary<AudioType, float> volumeLevels = 
        new() { 
            { AudioType.MASTER, 1.0f },
            { AudioType.MUSIC, 0.6f },
            { AudioType.SFX, 1.0f },
        };

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        volumeLevels[AudioType.MASTER] = PlayerPrefs.GetFloat("Audio: MASTER");
        volumeLevels[AudioType.MUSIC] = PlayerPrefs.GetFloat("Audio: MUSIC");
        volumeLevels[AudioType.SFX] = PlayerPrefs.GetFloat("Audio: SFX");

        for (int i = 0; i < tracks.Length; i++)
        {
            tracks[i].src = gameObject.AddComponent<AudioSource>();
            tracks[i].src.clip = tracks[i].clip;
            tracks[i].src.volume = tracks[i].Volume;
            tracks[i].src.pitch = tracks[i].Pitch;
            tracks[i].src.panStereo = tracks[i].Panning;
            tracks[i].src.loop = tracks[i].type == AudioType.MUSIC;
            tracks[i].src.outputAudioMixerGroup = AudOut;
        }

        if (tracksStatic.Length == 0)
        { tracksStatic = tracks; }

        NullInst.Name = null;
    }

    public void Update()
    { HandleFade(); }

    void HandleFade()
    {
        if (PreviousTrack.src != null)
        {
            if (PreviousTrack.src.volume > 0)
            {
                PreviousTrack.src.volume -= Time.unscaledDeltaTime * FADE_SPEED * (volumeLevels[AudioType.MASTER] + volumeLevels[AudioType.MUSIC]);
                if (PreviousTrack.src.volume <= 0)
                {  
                    PreviousTrack.src.Stop();  
                    PreviousTrack = NullInst;
                    
                }

            }

            if (PreviousTrack.Equals(NullInst))
            { return; }

            if (PreviousTrack.src.isPlaying) PreviousTrackTimer += Time.unscaledDeltaTime;
            float thresh = PreviousTrack.src.clip.samples / SAMPLERATE;
            if (PreviousTrackTimer > thresh - 0.05f)
            {
                PreviousTrackTimer = PreviousTrack.LoopPoint;
                PreviousTrack.src.time = PreviousTrackTimer;
            }
            else
            { PreviousTrackTimer = (PreviousTrack.src.timeSamples / SAMPLERATE); }
        }

        if (CurrentTrack.Name != null)
        {
            if (CurrentTrack.src.volume != volumeLevels[CurrentTrack.type] * volumeLevels[AudioType.MASTER])
            {
                CurrentTrack.src.volume += Time.unscaledDeltaTime * FADE_SPEED * volumeLevels[AudioType.MASTER];
                if (CurrentTrack.src.volume > volumeLevels[CurrentTrack.type] * volumeLevels[AudioType.MASTER])
                { CurrentTrack.src.volume = volumeLevels[CurrentTrack.type] * volumeLevels[AudioType.MASTER]; }
            }
            else if (CurrentTrack.src.volume >= volumeLevels[CurrentTrack.type] * volumeLevels[AudioType.MASTER])
            { CurrentTrack.src.volume -= Time.unscaledDeltaTime * FADE_SPEED; }

            if (CurrentTrack.src.isPlaying) 
            { CurrentTrackTimer += Time.unscaledDeltaTime; }

            float thresh = CurrentTrack.src.clip.samples / SAMPLERATE;

            if (CurrentTrackTimer >= thresh - 0.05f)
            {
                CurrentTrackTimer = CurrentTrack.LoopPoint;
                CurrentTrack.src.time = CurrentTrackTimer;
            }
            else
            { CurrentTrackTimer = (CurrentTrack.src.timeSamples / SAMPLERATE); }

        }
    }

    public static void ChangeVolume(float vol, AudioType type)
    { 
        volumeLevels[type] = vol; 
        for (int i = 0; i < tracksStatic.Length; i++) {

            if (tracksStatic[i].type == type) { 
                tracksStatic[i].src.volume = volumeLevels[tracksStatic[i].type];
                if (tracksStatic[i].type != AudioType.MASTER) 
                {tracksStatic[i].src.volume *= volumeLevels[AudioType.MASTER];}
            }
        }
    }

    public void PlayAudio(string name, float vol = 1.0f, float pitch = 0.0f, float pan = 0.5f)
    {
        AudioInstance aud = Array.Find(tracksStatic, tracks => tracks.Name == name);

        if (aud.src == null) 
        { 
            return;
        }

        if (pitch != 0.0f)
        { aud.src.pitch = pitch; }
        if (pan != 0.5f) 
        { aud.src.panStereo = pan; }
        if (vol != 0.0f)                                            //Keep volume at 0.0f if we want to fade audio in [I.E. music], otherwise set it to the current master volume * music volume
        {
            aud.src.volume = vol * volumeLevels[aud.type];
            if (aud.type != AudioType.MASTER)
            { aud.src.volume *= volumeLevels[AudioType.MASTER]; }
        }
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

        PreviousTrack = CurrentTrack;

        CurrentTrack = FindLooptrack(loopind, levelind);
        if (CurrentTrack.Name != null) 
        { PlayAudio(CurrentTrack.Name, 0.0f); }

        PreviousTrackTimer = CurrentTrackTimer;
        CurrentTrackTimer = 0;
    }
    
    public void FadeAllTracks()
    {
        if (CurrentTrack.Name == null) 
        { return; }
        PreviousTrack = CurrentTrack;
        CurrentTrack = NullInst;
    }

    public AudioInstance FindLooptrack(int loopind, int levelind) 
    { return Array.Find(tracksStatic, tracks => tracks.Name == LoopTrackList[levelind].Name[loopind]); }

    public AudioInstance FindTrack(string name) 
    { return Array.Find(tracksStatic, tracks => tracks.Name == name); }
}
