using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IconSwap : MonoBehaviour
{

    [SerializeField]
    private Sprite xbox;

    [SerializeField]
    private Sprite ps;

    [SerializeField]
    private Sprite keyboard;

    
    private string deviceName = "";
    private string controlSchemeName = "";

    private float blinkTimer = 0;

    [SerializeField]
    private float blinkSpeed = 0.75f;
    // Blink State
    private bool hasBlinked;

    [SerializeField]
    private bool canBlink = true;

    public void Update() {

        if (PlayerAI.inputRef != null)
        { ChangeIcon(); }
        
        if (!canBlink)
        { return; }

        // Blink
        if ((blinkTimer += Time.deltaTime) >= blinkSpeed) {

            blinkTimer = 0;
            GetComponent<Image>().color = (hasBlinked = !hasBlinked) ? Color.clear : Color.white;
           
        }

    }

    private void ChangeIcon() {
        
        if (PlayerAI.inputRef.currentControlScheme == controlSchemeName && PlayerAI.inputRef.devices[0].name == deviceName)
        { return; }

        controlSchemeName = PlayerAI.inputRef.currentControlScheme;
        deviceName = PlayerAI.inputRef.devices[0].name;

        if (PlayerAI.inputRef.currentControlScheme == "Keyboard")
        { GetComponent<Image>().sprite = keyboard; }
        else {
            if (InputSystem.IsFirstLayoutBasedOnSecond(PlayerAI.inputRef.devices[0].name, "DualShockGamepad"))
            { GetComponent<Image>().sprite = ps; }
            else
            { GetComponent<Image>().sprite = xbox; }
        }

        RectTransform rect = GetComponent<RectTransform>();

        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 100, 0);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75);

    }

    

}
