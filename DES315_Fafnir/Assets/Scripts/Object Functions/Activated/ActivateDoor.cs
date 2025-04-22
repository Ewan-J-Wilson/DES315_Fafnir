using UnityEngine;

public class ActivateDoor : ActivateGeneric
{
    // sprites to differentiate closed/open door
    [Tooltip("Sprite for when the door is closed")]
    [SerializeField]
    private Sprite closedSprite;

    [Tooltip("Sprite for when the door is open")]
    [SerializeField]
    private Sprite openSprite;

    override protected void DoAction() // opens door
    { 
        SpriteRenderer spriteRen = GetComponent<SpriteRenderer>();
        
        transform.position = new(transform.parent.position.x - (isActive ? 0.5f : 0f), transform.position.y); // offsets sprite difference

        spriteRen.sprite = isActive ? openSprite : closedSprite; // changes sprite accordingly
        GetComponent<BoxCollider2D>().enabled = !isActive;
    }
}
