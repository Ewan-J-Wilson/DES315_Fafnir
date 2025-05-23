using System.Collections;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class DialogueParser : MonoBehaviour {

    public static float ReadSpeed = 0.02f;
    private static string path;
    private StreamReader reader;
    public string processedBlock;
    [HideInInspector]
    public List<string> boxOutput;
    public static bool isReading;
    public static bool earlyBreak = false;


    void Start() {
        // Allows for multilingual support if needed
        path = Application.streamingAssetsPath + "/Dialogue/Game_GB.txt";
        reader = new(path);
    }

    public void ProcessChapter() {

        string line;

        if (!reader.EndOfStream)
        { line = reader.ReadLine(); }
        else {
            DialogueManager.DisablePlayerInput(false);
            gameObject.SetActive(false);
            return;
        }

        // Skip lines that are empty, are comments, or are before the start point
        if ((!line.Contains(DialogueManager.chapter) && !isReading) || line.Contains('#') || string.IsNullOrWhiteSpace(line))
        {
            ProcessChapter(); 
            return;
        }
        // Start Reading
        else if (!isReading) { 
            isReading = true;
            ProcessChapter(); 
            return;
        }

        // Add a space if there is one missing
        if (line[^1] != ' ' && line[^1] != ']' && line[^1] != '}')
        { line += ' '; }

         // End the block of dialogue
        if (line.Contains("END_DIAG")) {
            reader.Close();
            ReadBlock();
            return;
        }

        // Read the next line
        processedBlock += line;
        ProcessChapter();

    }

    public void ReadBlock() {

        // Clear the formatting
        bool format = false;
        string formatRead = "";

        int pageInd = 0;

        foreach (char c in processedBlock) {

            if (!(format || c == '{'))
            { boxOutput[pageInd] += c; }
            // Start Formatting
            else if (!format && c == '{')
            { format = true; }
            // Parse the text formatting
            else if (format && c == '}') {

                FormatToggles(formatRead);

                if (formatRead == "NEXT")
                { 
                    boxOutput.Add("");
                    pageInd++; }

                if (formatRead == "n")
                { boxOutput[pageInd] += "\n"; }

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
                    
                    boxOutput[pageInd] += "[" 
                        + ((PlayerAI.inputRef.currentControlScheme == "Keyboard") ? actionText[0] : actionText[1]) 
                        + "]";

                    if (actionMap == "Player") 
                    { PlayerAI.inputRef.SwitchCurrentActionMap("UI"); }

                }

                // Save for later
                if (formatRead.Contains("t:"))
                { boxOutput[pageInd] += "{" + formatRead + "}"; }
                
                formatRead = "";
                format = false;

            }
            // Read the format text
            else if (format)
            { formatRead += c; }

        }

        // Read Sections
        processedBlock = "";
        StartCoroutine(DialogueOutput());

    }

    public IEnumerator DialogueOutput() {

        TextMeshProUGUI textAsset = GetComponentInChildren<TextMeshProUGUI>();
        textAsset.text = "";

        bool format = false;
        string formatRead = "";

        /*
        TODO:
        - Check for out of bounds text
        */

        //for (int i = 0; i < boxOutput.Count; i++) {
        //
        //    textAsset.text = boxOutput[pageInd];
        //    if (textAsset.isTextOverflowing && boxOutput.Count > 1) {
        //
        //        boxOutput.Add(boxOutput[^1]);
        //        
        //
        //    }
        //
        //}

        bool isHTML = false;
        string HTMLTag = "";

        for (int i = 0; i < boxOutput.Count; i++) {

            foreach (char c in boxOutput[i]) {

                if (!(format || c == '[' || c == '{')) {

                    
                    // Skip HTML tags
                    if (c == '<') 
                    { isHTML = true; }

                    if (!isHTML)
                    { textAsset.text += c; }
                    else
                    { HTMLTag += c; }
                    
                    if (c == '>') { 
                        textAsset.text += HTMLTag;
                        isHTML = false; 
                        HTMLTag = "";
                    }

                    // Skip to the end of the panel
                    if (!DialogueManager.next || !DialogueManager.canSkip && !isHTML)
                    { yield return new WaitForSeconds(ReadSpeed); }

                }
                // Start formatting
                else if (!format && (c == '[' || c == '{'))
                { format = true; }
                // Parse the formatting
                else if (format && (c == ']' || c == '}')) {

                    // Sets the character icon
                    if (c == ']' && formatRead.Contains('_')) {

                        string iconPath = "/Character Icons/";
                        iconPath += formatRead.Split("_")[0] + "/";

                        Image character = GameObject.Find("Char Icon").GetComponent<Image>();
                        byte[] bytes = File.ReadAllBytes(Application.streamingAssetsPath + iconPath + formatRead + ".png");
                        character.sprite.texture.LoadImage(bytes);
                        
                    }
                    else if (c == ']')
                    { 
                        string undo = "[" + formatRead + "]";
                        foreach (char x in undo) {

                            textAsset.text += x;

                            // Skip to the end of the panel
                            if (!DialogueManager.next || !DialogueManager.canSkip && !isHTML)
                            { yield return new WaitForSeconds(ReadSpeed); }

                        }

                    }

                    if (c == '}') {

                        // Delay the text by the given number of seconds
                        if (formatRead.Contains("t:") && !DialogueManager.next)
                        { 
                            if (float.TryParse(formatRead.Split(":")[1], out float parsedTime))
                            { yield return new WaitForSeconds(parsedTime); }
                            else
                            { Debug.LogError("Cannot wait for " + formatRead.Split(":")[1] + " seconds"); }
                        }

                    }

                    // Clear the format parser
                    formatRead = "";
                    format = false;

                }
                else if (format)
                { formatRead += c; }

            }

            DialogueManager.next = false;

            if (i < boxOutput.Count - 1) {

                // Wait before moving to the next panel of dialogue
                if (DialogueManager.autoscroll)
                { yield return new WaitForSeconds(DialogueManager.autoscrollLength); }
                else
                { yield return new WaitUntil(ReadNext); }

                textAsset.text = "";

            }

        }

        boxOutput.Clear();

        DialogueManager.next = false;

        // Wait before moving to the next panel of dialogue
        if (DialogueManager.autoscroll)
        { yield return new WaitForSeconds(DialogueManager.autoscrollLength); }
        else
        { yield return new WaitUntil(ReadNext); }

        EndDialogue();

    }

    public void EndDialogue() {

        isReading = false;
        // Re-enable the player input and hide the dialogue box
        DialogueManager.DisablePlayerInput(false);
        gameObject.SetActive(false);

    }

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
        boxOutput.Add("");
        ProcessChapter();
    }

    public void FormatToggles(string formatRead) {

        // Disable Player Input if needed
        // no Enable due to it conflicting with the gamepad jump button
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
