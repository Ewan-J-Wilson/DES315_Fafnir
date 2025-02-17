using UnityEngine;

public class TriggerLever : TriggerGeneric {

    private bool isActive = false;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Tool"))
        { return; }

        isActive = !isActive;

        if (isActive)
        { OnTrigger(); }
        else
        { OnExit(); }

    }

}