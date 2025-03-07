using UnityEngine;

public class ActivateDoor : ActivateGeneric
{

    [Tooltip("TEMP: Colour for when the door is closed")]
    [SerializeField]
    private Color closedColour;
    [Tooltip("TEMP: Colour for when the door is open")]

    [SerializeField]
    private Color openColour;

    [SerializeField]
    protected string DoorSound;

    override protected void DoAction()
    {
      SpriteRenderer spriteRen = GetComponent<SpriteRenderer>();
        
       spriteRen.color = isActive ? openColour : closedColour;
        Audiomanager.instance.PlayAudio(isActive ? DoorSound : null);
         GetComponent<BoxCollider2D>().enabled = !isActive;
    }
}
