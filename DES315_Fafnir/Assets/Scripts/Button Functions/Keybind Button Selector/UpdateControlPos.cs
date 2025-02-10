using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;



public class UpdateControlPos : MonoBehaviour
{

    [Tooltip("Vertical Position in the Grid Layout")]
    public int gridIndex = 0;

    // Reference to the scrollbar
    [SerializeField]
    private Scrollbar scroll;

    // Number of button rows in the grid
    public static float gridNo = 0;
    
    // Only call GridInit on the first row to start
    public void Start() {
        if (gridNo == 0)
        { GridInit(GetComponentInParent<GridLayoutGroup>()); }
    }

    // Grab the number of button rows
    private static void GridInit(GridLayoutGroup _parent)
    { gridNo = _parent.GetComponentsInChildren<UpdateControlPos>().GetLength(0) - 1; }

    // Update the scrollbar position, scrolling the keybinds menu
    public void ScrollMove() 
    { scroll.value = 1 - (gridIndex / gridNo); }

}
