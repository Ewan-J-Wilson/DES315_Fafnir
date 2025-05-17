using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMenu : MonoBehaviour
{

    private bool readingText = false;

    // Update is called once per frame
    void Update()
    {
        
        if (readingText != DialogueRead.reading) {
        
            readingText = DialogueRead.reading;
            if (!readingText)
            { FindFirstObjectByType<GameManager>().NextLevel(); }

        
        }


    }
}
