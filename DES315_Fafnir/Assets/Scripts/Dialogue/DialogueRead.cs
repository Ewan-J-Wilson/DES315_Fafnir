using System.Collections;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class DialogueRead : MonoBehaviour
{
    
    public static float ReadSpeed = 0.02f;
    private static string path;
    private StreamReader reader;
    private string displayLine = "";
    public static bool reading = false;

    // Start is called before the first frame update
    void Start()
    {

        path = Application.streamingAssetsPath + "/Dialogue/Game_GB.txt";
        reader = new(path);

    }

    public void ReadBlock() {
        
        string line;
        if (!reader.EndOfStream) 
        { line = reader.ReadLine(); }
        else
        { 
            DialogueManager.DisablePlayerInput(false);
            gameObject.SetActive(false);
            return; 
        }

        if ((!line.Contains(DialogueManager.chapter) && !reading) || line.Contains('#') || string.IsNullOrWhiteSpace(line))
        { 
            ReadBlock(); 
            return;
        }
        else if (!reading) { 
            reading = true;
            ReadBlock(); 
            return;
        }
        
        if (line.Contains("END_DIAG")) {
            reader.Close();
            StartCoroutine(DisplayLine());
            return;
        }

        if (line[^1] != ' ' && line[^1] != ']' && line[^1] != '}')
        { line += ' '; }
        displayLine += line;
        ReadBlock();

    }


    public IEnumerator DisplayLine() {

        TextMeshProUGUI textAsset = GetComponentInChildren<TextMeshProUGUI>();
        textAsset.text = "";

        bool format = false;
        string formatRead = "";

        foreach (char c in displayLine) {

            if (!(format || c == '[' || c == '{')) 
            { 

                textAsset.text += c;
                yield return new WaitForSeconds(ReadSpeed);

            }
            else {

                if (format && (c == ']' || c == '}')) { 

                    if (c == ']') {

                        string iconPath = "/Character Icons/";
                        iconPath += formatRead.Split("_")[0] + "/";

                        Image character = GameObject.Find("Char Icon").GetComponent<Image>();
                        byte[] bytes = File.ReadAllBytes(Application.streamingAssetsPath + iconPath + formatRead + ".png");
                        character.sprite.texture.LoadImage(bytes);
                        
                    }

                    if (c == '}') {

                        if (formatRead == "NEXT") { 
                            if (DialogueManager.autoscroll)
                            { yield return new WaitForSeconds(DialogueManager.autoscrollLength); }
                            else
                            { yield return new WaitUntil(ReadNext); }
                            textAsset.text = "";
                        }

                        if (formatRead == "enable")
                        { DialogueManager.DisablePlayerInput(false); }

                        if (formatRead == "disable")
                        { DialogueManager.DisablePlayerInput(true); }

                        if (formatRead.Contains("t:"))
                        { yield return new WaitForSeconds(formatRead.Split(":")[1].ConvertTo<float>()); }

                        if (formatRead == "n")
                        { textAsset.text += "\n"; }

                    }

                    formatRead = "";
                    format = false;
                    
                }

                if (format) 
                { formatRead += c; }

                if (c == '[' || c == '{') 
                { format = true; }

            }

        }

        reading = false;
        displayLine = "";

        yield return new WaitUntil(ReadNext); 
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

    public void ReadStart() {

        reader = new(path);
        ReadBlock();

    }

}
