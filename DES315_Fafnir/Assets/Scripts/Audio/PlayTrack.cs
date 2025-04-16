using UnityEngine;
using UnityEngine.SceneManagement;

//Generic track player

public class PlayTrack : MonoBehaviour
{
    public string TrackName;

    private void Awake()
    {
        
        SceneManager.sceneLoaded += PlayMusic;

    }

    private void PlayMusic(Scene arg0, LoadSceneMode arg1) {

        if (Audiomanager.CurrentTrack.Name != TrackName)
        {
            if (Audiomanager.PreviousTrack.Equals(Audiomanager.NullInst))
            { Audiomanager.PreviousTrack = Audiomanager.CurrentTrack; }
            Audiomanager.instance.PlayAudio(TrackName, 0.0f);
            Audiomanager.CurrentTrack = FindFirstObjectByType<Audiomanager>().FindTrack(TrackName);
        }

    }

    private void OnDestroy() {

        SceneManager.sceneLoaded -= PlayMusic;

    }

}
