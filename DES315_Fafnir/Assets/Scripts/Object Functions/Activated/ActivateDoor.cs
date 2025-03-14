using UnityEngine;

public class ActivateDoor : ActivateGeneric
{

    [Tooltip("Sprite for when the door is closed")]
    [SerializeField]
    private Sprite closedSprite;

    [Tooltip("Sprite for when the door is open")]
    [SerializeField]
    private Sprite openSprite;

    override protected void DoAction()
    {
        SpriteRenderer spriteRen = GetComponent<SpriteRenderer>();
        
        transform.position = new(transform.parent.position.x - (isActive ? 0.5f : 0f), transform.position.y);

        spriteRen.sprite = isActive ? openSprite : closedSprite;
        GetComponent<BoxCollider2D>().enabled = !isActive;
    }
}
