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

    [SerializeField]
    private float IconSize = 75f;

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
        
        // Update the icon size
        RectTransform rect = GetComponent<RectTransform>();

        if (IconSize < 2000)
        { rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 60 * PlayerPrefs.GetFloat("Text Scale"), 0); }
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, IconSize * (IconSize < 2000 ? PlayerPrefs.GetFloat("Text Scale") : 1));
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, IconSize * (IconSize < 2000 ? PlayerPrefs.GetFloat("Text Scale") : 1));

        // Break early if the icon hasn't changed to save performance
        if (PlayerAI.inputRef.currentControlScheme == controlSchemeName && PlayerAI.inputRef.devices[0].name == deviceName)
        { return; }

        // Get the control scheme
        controlSchemeName = PlayerAI.inputRef.currentControlScheme;
        deviceName = PlayerAI.inputRef.devices[0].name;

        // Update the icon sprite based on the control scheme
        if (PlayerAI.inputRef.currentControlScheme == "Keyboard")
        { GetComponent<Image>().sprite = keyboard; }
        else {
            if (InputSystem.IsFirstLayoutBasedOnSecond(PlayerAI.inputRef.devices[0].name, "DualShockGamepad"))
            { GetComponent<Image>().sprite = ps; }
            else
            { GetComponent<Image>().sprite = xbox; }
        }

        // Re-update the icon if it changed
        if (IconSize < 2000)
        { rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 60 * PlayerPrefs.GetFloat("Text Scale"), 0); }
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, IconSize * (IconSize < 2000 ? PlayerPrefs.GetFloat("Text Scale") : 1));
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, IconSize * (IconSize < 2000 ? PlayerPrefs.GetFloat("Text Scale") : 1));

    }

    

}
