using System.Collections.Generic;
using Unity.VisualScripting;
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
  

    public void Start() {

        canSelect = true;

    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().sprite = 
            isActive ? (selected ? onSelectSprite : onSprite) :
                (selected ? offSelectSprite : offSprite);
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
            GameObject cursor = GameObject.FindGameObjectWithTag("Cursor");
            if (cursor != null) {
                if (GameObject.FindGameObjectWithTag("Cursor").TryGetComponent(out Tool_Swing tool)) 
                { tool.interactables.Remove(gameObject); }
            }

            selected = false;

        }

    }

}