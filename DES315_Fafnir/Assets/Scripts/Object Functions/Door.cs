using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeReference] private GameObject door;
    public Material closedMaterial; // red to indicate door is closed
    public Material openMaterial; // green to indicate door is open
    public bool isActive = false; // button default state 
    private Vector2 mouseOver; // position of cursor in the game window 
    private void UpdateMouse()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Default"))) // if mouse is within game window 
        {
            mouseOver.x = (int)(hit.point.x);
            mouseOver.y = (int)(hit.point.y);

            Debug.Log(Input.mousePosition); // prints cursor coordinates 
        }
        else // if mouse is outside game window 
        {
            mouseOver.x = -1; 
            mouseOver.y = -1;
            Debug.Log(Input.mousePosition);
        }

    }
    public void OnMouseDown()
    {
        var spriteRen = GetComponent<SpriteRenderer>();
        
        isActive = !isActive; // boolean that switches for current button state 
        spriteRen.material = isActive ? openMaterial : closedMaterial; // changes colour of button when clicked 

        door.SetActive(!door.activeSelf); // opens and closes door depending on button material and boolean state 
    }

    // Update is called once per frame
    void Update() // calls function to print mouse coordinates 
    {
        UpdateMouse();
    }
}
