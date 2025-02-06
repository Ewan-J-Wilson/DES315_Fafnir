using UnityEngine;

public class TriggerTest1 : TriggerChecker
{
    [SerializeReference] public GameObject door;
    public Material defaultMaterial;
    public Material openMaterial;
    public bool isActive = false;
    protected override void DoAction()
    {
        var spriteRen = GetComponent<SpriteRenderer>();

        isActive = !isActive;
        spriteRen.material = isActive ? openMaterial : defaultMaterial;
        door.SetActive(!door.activeSelf);
    }
}
