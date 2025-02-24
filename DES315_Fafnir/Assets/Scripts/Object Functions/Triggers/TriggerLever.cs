using Unity.VisualScripting;
using UnityEngine;

public class TriggerLever : TriggerGeneric {

    [SerializeField]
    private Color offColour;
    [SerializeField]
    private Color onColour;
  

    public void Start() {

        canSelect = true;

    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().color = selected ? onColour : offColour;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Cursor Radius")) 
        { return; }

        if (canSelect && collision.CompareTag("Cursor Radius")) {
            GameObject.FindGameObjectWithTag("Cursor").GetComponent<Tool_Swing>().interactables.Add(gameObject);
            
        }
        if (!collision.CompareTag("Tool"))
        { return; }

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