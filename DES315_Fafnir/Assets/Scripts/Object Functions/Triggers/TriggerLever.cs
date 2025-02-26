using Unity.VisualScripting;
using UnityEngine;

public class TriggerLever : TriggerGeneric {

    [SerializeField]
    [Tooltip("TEMP: Colour for when the lever is off and deselected")]
    private Color offColour;
    [SerializeField]
    [Tooltip("TEMP: Colour for when the lever is on and deselected")]
    private Color onColour;

    [SerializeField]
    [Tooltip("TEMP: Colour for when the lever is off and currently selected")]
    private Color offSelectColour;
    [SerializeField]
    [Tooltip("TEMP: Colour for when the lever is on and currently deselected")]
    private Color onSelectColour;
  

    public void Start() {

        canSelect = true;

    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().color = 
            isActive ? (selected ? onSelectColour : onColour) :
                (selected ? offSelectColour : offColour);
    }


    
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Cursor Radius")) 
        { return; }

        if (canSelect) {
            foreach (GameObject cursor in GameObject.FindGameObjectsWithTag("Cursor"))
            { 
                if (!cursor.GetComponent<Tool_Swing>().interactables.Contains(gameObject))
                { cursor.GetComponent<Tool_Swing>().interactables.Add(gameObject); }
            }
            
        }

    }

    protected void OnTriggerExit2D(Collider2D collision)
    {

        if (!collision.CompareTag("Cursor Radius"))
        { return; }


        if (canSelect)
        {
            
            GameObject.FindGameObjectWithTag("Cursor").GetComponent<Tool_Swing>().interactables.Remove(gameObject);
            selected = false;

        }

    }

}