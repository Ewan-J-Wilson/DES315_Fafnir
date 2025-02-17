using UnityEngine;

public class ActivateDoor : ActivateGeneric
{

    public Material closedMaterial;
    public Material openMaterial;

    override protected void DoAction()
    {
        SpriteRenderer spriteRen = GetComponent<SpriteRenderer>();
 
        spriteRen.material = isActive ? openMaterial : closedMaterial;
        GetComponent<BoxCollider2D>().enabled = !isActive;
    }
}
