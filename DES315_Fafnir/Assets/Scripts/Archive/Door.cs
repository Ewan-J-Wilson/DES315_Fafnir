using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeReference] private GameObject door;
    public Material closedMaterial;
    public Material openMaterial;
    public bool isActive = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Tool"))
        {
            InvertDoorState();
        }
    }
    

    public void InvertDoorState()
    {
        SpriteRenderer spriteRen = GetComponent<SpriteRenderer>();

        isActive = !isActive;
        spriteRen.material = isActive ? openMaterial : closedMaterial;

        door.SetActive(!door.activeSelf);
    }
}
