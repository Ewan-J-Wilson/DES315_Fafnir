using UnityEngine;

public class TriggerButton : TriggerGeneric
{

    [SerializeField]
    [Tooltip("TEMP: Colour for when the button is off")]
    private Sprite offSprite;
    [SerializeField]
    [Tooltip("TEMP: Colour for when the button is on")]
    private Sprite onSprite;

    private bool trigger = false;


    private void Update()
    {

        GetComponent<SpriteRenderer>().sprite = isActive ? onSprite : offSprite;

        // The frame the button is pressed down
        if (!trigger && isActive) { 
            OnTrigger();
            trigger = true;
        }
        // The frame the button is released
        else if (trigger && !isActive) { 
            OnExit(); 
            trigger = false;
        }

    }

    // Detects the player on the button
    private void OnTriggerStay2D(Collider2D collision) { 
        if (collision.CompareTag("Player") || collision.CompareTag("Clone")) 
        { isActive = true; }
    }

    // Detects the player leaving the button
    private void OnTriggerExit2D(Collider2D collision) { 
        if (collision.CompareTag("Player") || collision.CompareTag("Clone")) 
        { isActive = false; }
    }
}
