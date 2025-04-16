using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        if (!PlayerPrefs.HasKey("Audio: MASTER"))
        { PlayerPrefs.SetFloat("Audio: MASTER", 0.5f); }

        if (!PlayerPrefs.HasKey("Audio: MUSIC"))
        { PlayerPrefs.SetFloat("Audio: MUSIC", 0.5f); }

        if (!PlayerPrefs.HasKey("Audio: SFX"))
        { PlayerPrefs.SetFloat("Audio: SFX", 0.5f); }

        if (!PlayerPrefs.HasKey("Text Scale"))
        { PlayerPrefs.SetFloat("Text Scale", 1f); }

        PlayerPrefs.Save();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
