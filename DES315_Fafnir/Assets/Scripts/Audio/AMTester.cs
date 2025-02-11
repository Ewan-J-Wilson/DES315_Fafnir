using UnityEngine;

public class AMTester : MonoBehaviour
{
    private float ptime = 0;

    // Update is called once per frame
    void Update()
    {
        ptime += Time.deltaTime;
        if ( ptime > 1.0f )
        {
            ptime = 0.0f;
            //Audiomanager.instance.PlayAudio("Kick");
            Audiomanager.instance.PlayAudio("Kick", 0.25f, 1, 0.5f);
        }
    }
}
