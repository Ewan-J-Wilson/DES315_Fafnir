using UnityEngine;
using UnityEngine.EventSystems;

public class ControlsFirstSelected : MonoBehaviour
{

    [SerializeField] private GameObject keyboardFirstSelected;
    [SerializeField] private GameObject gamepadFirstSelected;

    // Start is called before the first frame update
    void Awake()
    {
        FindFirstObjectByType<EventSystem>().firstSelectedGameObject = 
            MenuButtons.swapCondition ?
            gamepadFirstSelected :
            keyboardFirstSelected;
    }

}
