using UnityEngine;

//Generic track player

public class PlayTrack : MonoBehaviour
{
    public string TrackName;

    private void Start()
    {
        if (FindFirstObjectByType<Audiomanager>().CurrentTrack.Name != TrackName)
        {
            FindFirstObjectByType<Audiomanager>().PlayAudio(TrackName, 0.0f);
            FindFirstObjectByType<Audiomanager>().PreviousTrack = FindFirstObjectByType<Audiomanager>().NullInst;
            FindFirstObjectByType<Audiomanager>().CurrentTrack = FindFirstObjectByType<Audiomanager>().FindTrack(TrackName);
        }
    }
}
