using UnityEngine;

public class TriggerLever : TriggerGeneric {

    private bool isActive = false;
    [SerializeField]
    private Color offColour;
    [SerializeField]
    private Color onColour;

    public void Start() {

        canSelect = true;

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Tool"))
        { return; }

        //isActive = !isActive;
        //GetComponent<SpriteRenderer>().color = isActive ? onColour : offColour;

        //if (isActive)
        //{ OnTrigger(); }
        //else
        //{ OnExit(); }

    }

}