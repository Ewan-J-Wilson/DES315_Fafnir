using UnityEngine;

public class ActivateDoor : ActivateGeneric
{

    public Material closedMaterial;
    public Material openMaterial;
    //public bool isActive = false;

    override protected void DoAction()
    {
        SpriteRenderer spriteRen = GetComponent<SpriteRenderer>();
 
        spriteRen.material = isActive ? openMaterial : closedMaterial;
        GetComponent<BoxCollider2D>().enabled = !isActive;
    }
}
