using Unity.VisualScripting;
using UnityEngine;

public class TriggerLever : TriggerGeneric {

    [SerializeField]
    private Color offColour;
    [SerializeField]
    private Color onColour;

    [SerializeField]
    private Color offSelectColour;
    [SerializeField]
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

        //isActive = !isActive;
        //GetComponent<SpriteRenderer>().color = isActive ? onColour : offColour;

        //if (isActive)
        //{ OnTrigger(); }
        //else
        //{ OnExit(); }

    }

    protected void OnTriggerExit2D(Collider2D collision)
    {

        if (!collision.CompareTag("Cursor Radius"))
        { return; }


        Debug.Log("exit");
        if (canSelect && collision.CompareTag("Cursor Radius"))
        {
            
            GameObject.FindGameObjectWithTag("Cursor").GetComponent<Tool_Swing>().interactables.Remove(gameObject);
            selected = false;

        }

    }

}