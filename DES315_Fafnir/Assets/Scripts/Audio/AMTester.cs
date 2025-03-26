using UnityEngine;

public class AMTester : MonoBehaviour
{
    Audiomanager am;
    private float ptime = 0;
    private int loopind = 0;

    private void Start()
    {
        am = FindFirstObjectByType<Audiomanager>();
    }

    // Update is called once per frame
    void Update()
    {
        ptime += Time.deltaTime;
        if ( ptime > 1.0f )
        {
            ptime = 0.0f;
            //Audiomanager.instance.PlayAudio("Kick");
            //Audiomanager.instance.PlayAudio("Kick", 0.25f, 1, 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            loopind--;
            loopind = Mathf.Clamp(loopind, 0, 6);
            if (loopind >= 3) am.FadeLoopTracks(loopind - 3, 1);
            else am.FadeLoopTracks(loopind, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            loopind++;
            loopind = Mathf.Clamp(loopind, 0, 6);
            if (loopind >= 3) am.FadeLoopTracks(loopind-3, 1);
            else am.FadeLoopTracks(loopind, 0);
        }
    }
}
