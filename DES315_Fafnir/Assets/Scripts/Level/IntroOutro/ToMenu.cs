using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMenu : MonoBehaviour
{

    private bool readingText = false;

    // Update is called once per frame
    void Update()
    {
        
        if (readingText != DialogueParser.isReading) {
        
            readingText = DialogueParser.isReading;
            if (!readingText)
            { FindFirstObjectByType<GameManager>().NextLevel(); }

        
        }


    }
}
