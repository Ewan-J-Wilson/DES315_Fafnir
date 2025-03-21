using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;
using static UnityEngine.InputSystem.InputBinding;

public class IconSwap : MonoBehaviour
{

    [SerializeField]
    private Sprite xbox;

    [SerializeField]
    private Sprite ps;

    [SerializeField]
    private Sprite keyboard;

    private PlayerInput input;
    private string deviceName = "";
    private string controlSchemeName = "";

    private float blinkTimer = 0;

    [SerializeField]
    private float blinkSpeed = 0.75f;
    // Blink State
    private bool hasBlinked;

    [SerializeField]
    private bool canBlink = true;

    public void Start() {

        SceneManager.sceneLoaded += DetectInputScheme;
        DetectInputScheme(gameObject.scene, LoadSceneMode.Single);

        ChangeIcon();

    }

    public void Update() {

        ChangeIcon();
        
        if (!canBlink)
        { return; }

        // Blink
        if ((blinkTimer += Time.deltaTime) >= blinkSpeed) {

            blinkTimer = 0;
            GetComponent<Image>().color = (hasBlinked = !hasBlinked) ? Color.clear : Color.white;
           
        }

    }

    private void ChangeIcon() {
        
        if (input.currentControlScheme == controlSchemeName && input.devices[0].name == deviceName)
        { return; }

        controlSchemeName = input.currentControlScheme;
        deviceName = input.devices[0].name;

        if (input.currentControlScheme == "Keyboard")
        { GetComponent<Image>().sprite = keyboard; }
        else {
            if (InputSystem.IsFirstLayoutBasedOnSecond(input.devices[0].name, "DualShockGamepad"))
            { GetComponent<Image>().sprite = ps; }
            else
            { GetComponent<Image>().sprite = xbox; }
        }

        RectTransform rect = GetComponent<RectTransform>();

        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 50, 0);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);

    }

    public void DetectInputScheme(Scene scene,LoadSceneMode load) {

        if (!GameObject.FindGameObjectWithTag("Player").TryGetComponent(out input)) {

            GetComponent<PlayerInput>().enabled = true;
            input = GetComponent<PlayerInput>();

        }
        else
        { GetComponent<PlayerInput>().enabled = false; }

    }

}
