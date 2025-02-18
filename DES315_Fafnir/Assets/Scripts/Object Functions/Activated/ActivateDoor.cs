using UnityEngine;

public class ActivateDoor : ActivateGeneric
{

    [SerializeField]
    private Color closedColour;
    [SerializeField]
    private Color openColour;

    override protected void DoAction()
    {
        SpriteRenderer spriteRen = GetComponent<SpriteRenderer>();
 
        spriteRen.color = isActive ? openColour : closedColour;
        GetComponent<BoxCollider2D>().enabled = !isActive;
    }
}
