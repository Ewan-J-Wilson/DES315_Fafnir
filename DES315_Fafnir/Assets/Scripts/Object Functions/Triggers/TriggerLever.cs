using UnityEngine;

public class TriggerLever : TriggerGeneric { // used for levers 

    // four different sprites based on on/off status and whether or not it can be selected
    [SerializeField]
    [Tooltip("Sprite for when the lever is off and deselected")]
    private Sprite offSprite;
    [SerializeField]
    [Tooltip("Sprite for when the lever is on and deselected")]
    private Sprite onSprite;

    [SerializeField]
    [Tooltip("Sprite for when the lever is off and currently selected")]
    private Sprite offSelectSprite;
    [SerializeField]
    [Tooltip("Sprite for when the lever is on and currently deselected")]
    private Sprite onSelectSprite;

    private bool spriteSelected = false; // initial value for whether or not sprite is selected

    public void Start() { // enables selection

        canSelect = true;

    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().sprite = 
            isActive ? (spriteSelected ? onSelectSprite : onSprite) :
                (spriteSelected ? offSelectSprite : offSprite);

    }

    
    protected void OnTriggerEnter2D(Collider2D collision) // lever enters cursor radius, thus it can be selected
    {
        if (!collision.CompareTag("Cursor Radius")) 
        { return; }

        // if lever can be selected, it is possible to change its on/off status
        if (canSelect) {
            foreach (GameObject cursor in GameObject.FindGameObjectsWithTag("Cursor"))
            { 
                
                if (!cursor.GetComponent<Tool_Swing>().interactables.Contains(gameObject)) { 

                    if (collision.transform.parent.CompareTag("Player") 
                       && collision.transform == cursor.transform.parent)
                    { spriteSelected = true; }

                    cursor.GetComponent<Tool_Swing>().interactables.Add(gameObject); 
                    
                }
            }
            
        }

    }

    protected void OnTriggerExit2D(Collider2D collision) // lever exits cursor radius, thus cannot be selected
    {
        // if lever cannot be selected, it is impossible to change its on/off status
        if (!collision.CompareTag("Cursor Radius")) 
        { return; }


        if (canSelect) 
        {
            if (GameObject.FindGameObjectWithTag("Cursor") != null)
            {
                if (GameObject.FindGameObjectWithTag("Cursor").TryGetComponent(out Tool_Swing tool))
                {

                    if (collision.transform.parent.CompareTag("Player")
                        && collision.transform == tool.transform.parent)
                    { spriteSelected = false; }

                    tool.interactables.Remove(gameObject);

                }

                selected = false;
            }
        }

    }

}