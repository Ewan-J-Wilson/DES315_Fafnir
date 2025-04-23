using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneIndicator : MonoBehaviour
{

    [SerializeField]
    private GameObject image;
    private PlayerAI player;
    private readonly List<Image> playerSprites = new();

    [SerializeField]
    private float blinkSpeed = 0.33f;
    private float blinkTimer = 0;
    // Blink State
    private bool hasBlinked;

    [SerializeField]
    private Color blinkColour;

    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerAI>();
        foreach (Image c in GetComponentsInChildren<Image>()) 
        { playerSprites.Add(c); }

        if (playerSprites.Count != 4)
        { Debug.LogError("This component does not have the correct number of sprites attached to it"); }

        for (int i = 1; i <= 4; i++) 
        { playerSprites[^i].enabled = false; }


    }

    // Update is called once per frame
    void Update()
    {

        // Loop through each sprite
        for (int i = 1; i <= 4; i++) {

            if (FindFirstObjectByType<Fade>().IsFading()) 
            { playerSprites[^i].enabled = false; }
            else if (!playerSprites[^i].enabled)
            { playerSprites[^i].enabled = true; }

            if (player.IsRecording && i - 1 == player.CloneNo) {
                //Debug.LogWarning("Blinking");
                // Blink
                if ((blinkTimer += Time.deltaTime) >= blinkSpeed) {

                    blinkTimer = 0;
                    playerSprites[^i].color = (hasBlinked = !hasBlinked) ? blinkColour : Color.white;
                   
                }

            }
            else {

                if (i <= player.CloneNo)
                { playerSprites[^i].color = Color.clear; }
                else if (i > player.CloneNo)
                { playerSprites[^i].color = Color.white; }

            }

        }

    }
}
