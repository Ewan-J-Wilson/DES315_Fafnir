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
            Audiomanager.instance.PlayAudio(TrackName, 0.0f);
            Audiomanager.PreviousTrack = Audiomanager.NullInst;
            Audiomanager.CurrentTrack = FindFirstObjectByType<Audiomanager>().FindTrack(TrackName);
        }

    }

    private void OnDestroy() {

        SceneManager.sceneLoaded -= PlayMusic;

    }

}
