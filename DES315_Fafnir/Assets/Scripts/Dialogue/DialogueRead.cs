using System.Collections;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DialogueRead : MonoBehaviour
{
    
    public static float ReadSpeed = 0.02f;
    private static string path;
    private StreamReader reader;
    private string displayLine = "";
    public static bool reading = false;

    public Dictionary<string, string> buttonReplace = new(){
        { "XButton South",  "A" },
        { "XButton East",   "B" },
        { "XButton North",  "Y" },
        { "XButton West",   "X" },
        { "PSButton South", "X" },
        { "PSButton East",  "○" },
        { "PSButton North", "△" },
        { "PSButton West",  "□" }
    };

    // On Start, get the dialogue file
    void Start()
    {
        path = Application.streamingAssetsPath + "/Dialogue/Game_GB.txt";
        reader = new(path);
    }

    //  Read the block of text
    public void ReadBlock() {

        // Initialise the reading string
        string line;

        // If this is not the end of the file, parse the line
        if (!reader.EndOfStream) 
        { line = reader.ReadLine(); }
        // Other stop reading
        else
        { 
            DialogueManager.DisablePlayerInput(false);
            gameObject.SetActive(false);
            return; 
        }

        // Skip lines that are empty, are comments, or are before the start point
        if ((!line.Contains(DialogueManager.chapter) && !reading) || line.Contains('#') || string.IsNullOrWhiteSpace(line))
        {
            ReadBlock(); 
            return;
        }
        // Start Reading
        else if (!reading) { 
            reading = true;
            ReadBlock(); 
            return;
        }
        
        // End the block of dialogue
        if (line.Contains("END_DIAG")) {
            reader.Close();
            StartCoroutine(DisplayLine());
            return;
        }

        // Add a space if there is one missing
        if (line[^1] != ' ' && line[^1] != ']' && line[^1] != '}')
        { line += ' '; }

        // Read the next line
        displayLine += line;
        ReadBlock();

    }

    // Parse the current line
    public IEnumerator DisplayLine() {

        // Get the text object
        TextMeshProUGUI textAsset = GetComponentInChildren<TextMeshProUGUI>();
        textAsset.text = "";

        // Clear the formatting
        bool format = false;
        string formatRead = "";

        foreach (char c in displayLine) {

            // Read the text
            if (!(format || c == '[' || c == '{')) 
            { 

                textAsset.text += c;

                // Skip to the end of the panel
                if (!DialogueManager.next || !DialogueManager.canSkip)
                { yield return new WaitForSeconds(ReadSpeed); }

            }
            // Start formatting
            else if (!format && (c == '[' || c == '{'))
            { format = true; }
            // Parse the formatting
            else if (format && (c == ']' || c == '}')) {

                // Sets the character icon
                if (c == ']') {

                    string iconPath = "/Character Icons/";
                    iconPath += formatRead.Split("_")[0] + "/";

                    Image character = GameObject.Find("Char Icon").GetComponent<Image>();
                    byte[] bytes = File.ReadAllBytes(Application.streamingAssetsPath + iconPath + formatRead + ".png");
                    character.sprite.texture.LoadImage(bytes);
                    
                }

                // Parse the text formatting
                if (c == '}') {

                    // Move to the next panel of dialogue
                    if (formatRead == "NEXT") { 

                        // Prevent accidental dialogue skips
                        DialogueManager.next = false;

                        // Either wait for autoscroll, or allow the player to skip immediately
                        if (DialogueManager.autoscroll)
                        { yield return new WaitForSeconds(DialogueManager.autoscrollLength); }
                        else
                        { yield return new WaitUntil(ReadNext); }

                        //Clear the textbox
                        textAsset.text = "";

                    }

                    if (formatRead.Contains("Player/") || formatRead.Contains("UI/")) {

                        string actionMap = formatRead.Split("/")[0];
                        string action = formatRead.Split("/")[1];

                        if (actionMap == "Player") 
                        { PlayerAI.inputRef.SwitchCurrentActionMap("Player"); }

                        List<string> actionText = new();
                        for (int i = 0; i < PlayerAI.inputRef.currentActionMap.FindAction(action, true).bindings.Count; i++) { 
                            
                            string actionName = PlayerAI.inputRef.currentActionMap.FindAction(action, true).GetBindingDisplayString(i);
                            if (actionName.Contains("/") || PlayerAI.inputRef.currentActionMap.FindAction(action, true).bindings.Count < 3)
                            { actionText.Add(actionName); }
                            
                        }

                        textAsset.text += "[" 
                            + ((PlayerAI.inputRef.currentControlScheme == "Keyboard") ? actionText[0] : actionText[1]) 
                            + "]";

                        if (actionMap == "Player") 
                        { PlayerAI.inputRef.SwitchCurrentActionMap("UI"); }

                    }

                    // Delay the text by the given number of seconds
                    if (formatRead.Contains("t:") && !DialogueManager.next)
                    { 
                        if (float.TryParse(formatRead.Split(":")[1], out float parsedTime))
                        { yield return new WaitForSeconds(parsedTime); }
                        else
                        { Debug.LogError("Cannot wait for " + formatRead.Split(":")[1] + " seconds"); }
                    }

                    // Force a new line
                    if (formatRead == "n")
                    { textAsset.text += "\n"; }

                    // Check dialogue settings
                    FormatToggles(formatRead);

                }

                // Clear the format parser
                formatRead = "";
                format = false;

            }
            // Read the format text
            else if (format)
            { formatRead += c; }

        }

        
        displayLine = "";

        DialogueManager.next = false;

        // Wait before moving to the next panel of dialogue
        if (DialogueManager.autoscroll)
        { yield return new WaitForSeconds(DialogueManager.autoscrollLength); }
        else
        { yield return new WaitUntil(ReadNext); }

        reading = false;
        // Re-enable the player input and hide the dialogue box
        DialogueManager.DisablePlayerInput(false);
        gameObject.SetActive(false);

    }

    public void EndDialogue() {

        reading = false;
        // Re-enable the player input and hide the dialogue box
        DialogueManager.DisablePlayerInput(false);
        gameObject.SetActive(false);

    }

    // Check for the next panel / skip dialogue prompt
    private bool ReadNext() { 
        if (DialogueManager.next) {
            DialogueManager.next = false;
            return true;
        }
        return false; 
    }

    // Start reading the text
    public void ReadStart() {
        reader = new(path);
        ReadBlock();
    }

    // Toggles Dialogue settings on or off
    public void FormatToggles(string formatRead) {

        // Whether to let the player move during dialogue
        if (formatRead == "input enable")
        { DialogueManager.DisablePlayerInput(false); }
        if (formatRead == "input disable")
        { DialogueManager.DisablePlayerInput(true); }

        // Whether to allow autoscroll
        if (formatRead == "scroll start")
        { DialogueManager.autoscroll = true; }
        if (formatRead == "scroll stop")
        { DialogueManager.autoscroll = false; }

        // Whether to allow manual skipping through dialogue
        if (formatRead == "skip enable")
        { DialogueManager.canSkip = true; }
        if (formatRead == "skip disable")
        { DialogueManager.canSkip = false; }

    }

}
