using System;
using UnityEngine.UI;

////TODO: have updateBindingUIEvent receive a control path string, too (in addition to the device layout name)

namespace UnityEngine.InputSystem.Samples.RebindUI
{
    /// <summary>
    /// This is an example for how to override the default display behavior of bindings. The component
    /// hooks into <see cref="RebindActionUI.updateBindingUIEvent"/> which is triggered when UI display
    /// of a binding should be refreshed. It then checks whether we have an icon for the current binding
    /// and if so, replaces the default text display with an icon.
    /// </summary>
    public class KeyboardIconsExample : MonoBehaviour
    {
        public KeyboardIcons keyboard;

        protected void OnEnable()
        {
            // Hook into all updateBindingUIEvents on all RebindActionUI components in our hierarchy.
            var rebindUIComponents = transform.GetComponentsInChildren<RebindActionUI>();
            foreach (var component in rebindUIComponents)
            {
                component.updateBindingUIEvent.AddListener(OnUpdateBindingDisplay);
                component.UpdateBindingDisplay();
            }
        }

        protected void OnUpdateBindingDisplay(RebindActionUI component, string bindingDisplayString, string deviceLayoutName, string controlPath)
        {
            if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(controlPath))
                return;

            var icon = keyboard.GetSprite(controlPath);

            var textComponent = component.bindingText;

            // Grab Image component.
            var imageGO = textComponent.transform.parent.Find("ActionBindingIcon");
            var imageRect = imageGO.GetComponent<RectTransform>();
            var imageComponent = imageGO.GetComponent<Image>();

            if (icon != null)
            {
                textComponent.gameObject.SetActive(false);
                imageComponent.sprite = icon;
                imageComponent.gameObject.SetActive(true);
            }
            else
            {
                textComponent.gameObject.SetActive(true);
                imageComponent.gameObject.SetActive(false);
            }

            imageRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 25, 0);
            imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50);
            imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);

        }

        [Serializable]
        public struct KeyboardIcons
        {

            public Sprite mouseL;
            public Sprite mouseR;
            public Sprite mouseM;

            public Sprite key1;
            public Sprite key2;
            public Sprite key3;
            public Sprite key4;
            public Sprite key5;
            public Sprite key6;
            public Sprite key7;
            public Sprite key8;
            public Sprite key9;
            public Sprite key0;

            public Sprite keyQ;
            public Sprite keyW;
            public Sprite keyE;
            public Sprite keyR;
            public Sprite keyT;
            public Sprite keyY;
            public Sprite keyU;
            public Sprite keyI;
            public Sprite keyO;
            public Sprite keyP;

            public Sprite keyA;
            public Sprite keyS;
            public Sprite keyD;
            public Sprite keyF;
            public Sprite keyG;
            public Sprite keyH;
            public Sprite keyJ;
            public Sprite keyK;
            public Sprite keyL;

            public Sprite keyZ;
            public Sprite keyX;
            public Sprite keyC;
            public Sprite keyV;
            public Sprite keyB;
            public Sprite keyN;
            public Sprite keyM;

            public Sprite keySemicolon;
            public Sprite keySlash;
            public Sprite keyPlus;
            public Sprite keyMinus;
            public Sprite keyAsterisk;

            public Sprite keyCtrl;
            public Sprite keyShift;
            public Sprite keyAlt;
            public Sprite keyEnter;
            public Sprite keySpace;

            public Sprite keyUp;
            public Sprite keyDown;
            public Sprite keyLeft;
            public Sprite keyRight;
            

            public Sprite GetSprite(string controlPath)
            {
                // From the Input system, we get the path of the control on device. So we can just
                // map from that to the sprites we have for gamepads.
                switch (controlPath)
                {
                    case "0": return key0;
                    case "1": return key1;
                    case "2": return key2;
                    case "3": return key3;
                    case "4": return key4;
                    case "5": return key5;
                    case "6": return key6;
                    case "7": return key7;
                    case "8": return key8;
                    case "9": return key9;

                    case "numpad0": return key0;
                    case "numpad1": return key1;
                    case "numpad2": return key2;
                    case "numpad3": return key3;
                    case "numpad4": return key4;
                    case "numpad5": return key5;
                    case "numpad6": return key6;
                    case "numpad7": return key7;
                    case "numpad8": return key8;
                    case "numpad9": return key9;

                    case "leftButton": return mouseL;
                    case "rightButton": return mouseR;
                    case "middleButton": return mouseM;

                    case "q": return keyQ;
                    case "w": return keyW;
                    case "e": return keyE;
                    case "r": return keyR;
                    case "t": return keyT;
                    case "y": return keyY;
                    case "u": return keyU;
                    case "i": return keyI;
                    case "o": return keyO;
                    case "p": return keyP;
                    case "a": return keyA;
                    case "s": return keyS;
                    case "d": return keyD;
                    case "f": return keyF;
                    case "g": return keyG;
                    case "h": return keyH;
                    case "j": return keyJ;
                    case "k": return keyK;
                    case "l": return keyL;
                    case "z": return keyZ;
                    case "x": return keyX;
                    case "c": return keyC;
                    case "v": return keyV;
                    case "b": return keyB;
                    case "n": return keyN;
                    case "m": return keyM;

                    case "semicolon": return keySemicolon;
                    case "slash": return keySlash;
                    case "plus": return keyPlus;
                    case "minus": return keyMinus;
                    case "asterisk": return keyAsterisk;

                    case "leftCtrl": return keyCtrl;
                    case "leftShift": return keyShift;
                    case "leftAlt": return keyAlt;
                    case "enter": return keyEnter;
                    case "space": return keySpace;

                    case "upArrow": return keyUp;
                    case "downArrow": return keyDown;
                    case "leftArrow": return keyLeft;
                    case "rightArrow": return keyRight;

                    
                }
                return null;
            }
        }

    }
}
