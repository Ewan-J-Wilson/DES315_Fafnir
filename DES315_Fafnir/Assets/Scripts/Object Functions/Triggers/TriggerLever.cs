using UnityEngine;

public class TriggerLever : TriggerGeneric {

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

    private bool spriteSelected = false;

    

    public void Start() {

        canSelect = true;

    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().sprite = 
            isActive ? (spriteSelected ? onSelectSprite : onSprite) :
                (spriteSelected ? offSelectSprite : offSprite);

    }

    
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Cursor Radius")) 
        { return; }

        if (canSelect) {
            foreach (GameObject cursor in GameObject.FindGameObjectsWithTag("Cursor"))
            { 
                
                if (!cursor.GetComponent<Tool_Swing>().interactables.Contains(gameObject)) { 

                    //Debug.LogWarning(collision);
                    //Debug.LogWarning(cursor.transform.parent);
                    if (collision.transform.parent.CompareTag("Player") 
                       && collision.transform == cursor.transform.parent)
                    { spriteSelected = true; }

                    cursor.GetComponent<Tool_Swing>().interactables.Add(gameObject); 
                    
                }
            }
            
        }

    }

    protected void OnTriggerExit2D(Collider2D collision)
    {

        if (!collision.CompareTag("Cursor Radius"))
        { return; }


        if (canSelect)
        {
            if (GameObject.FindGameObjectWithTag("Cursor").TryGetComponent(out Tool_Swing tool)) { 
                    
                    if (collision.transform.parent.CompareTag("Player") 
                        && collision.transform == tool.transform.parent)
                    { spriteSelected = false; }
                    //Debug.LogWarning(tool.transform.parent);
                    tool.interactables.Remove(gameObject); 

            }

            selected = false;

        }

    }

}