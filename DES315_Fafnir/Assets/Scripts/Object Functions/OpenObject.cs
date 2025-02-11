using UnityEngine;

public class OpenObject : MonoBehaviour
{
    // Flag for if the object is open
    [Tooltip("Flag for if the object is open")]
    public bool open = false;
    [HideInInspector] public int counter = 0;
    // Number of buttons to open object
    [Tooltip("Number of Buttons to open object")]
    [Range(1,4)][SerializeField] protected int threshold = 1;
    // TODO: replace with open and closed textures
    [SerializeField] protected Color closedColour;
    [SerializeField] protected Color openColour;

    // Update is called once per frame
    void FixedUpdate()
    {
       
        // If the button is pressed or released, trigger the open script
        if (open != (counter >= threshold)) {
            open = (counter >= threshold);
            OpenScript(open);
        }

    }

    // Script to open/close the object
    virtual public void OpenScript(bool _getOpen) {

        GetComponent<BoxCollider2D>().enabled = !_getOpen;
        GetComponent<SpriteRenderer>().color = _getOpen ? openColour : closedColour;
    }
}
