using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeReference] private GameObject door;
    public Material closedMaterial;
    public Material openMaterial;
    public bool isActive = false;
    private Vector2 mouseOver;

    private void UpdateMouse()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Default")))
        {
            mouseOver.x = (int)(hit.point.x);
            mouseOver.y = (int)(hit.point.y);

            Debug.Log(Input.mousePosition);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
            Debug.Log(Input.mousePosition);
        }

    }
    public void OnMouseDown()
    {
        InvertDoorState();
    }

    public void InvertDoorState()
    {
        var spriteRen = GetComponent<SpriteRenderer>();

        isActive = !isActive;
        spriteRen.material = isActive ? openMaterial : closedMaterial;

        door.SetActive(!door.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouse();
    }
}
