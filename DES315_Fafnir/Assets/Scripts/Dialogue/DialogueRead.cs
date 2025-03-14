using System.Collections;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Timeline;

public class DialogueRead : MonoBehaviour
{
    
    public static float ReadSpeed = 0.02f;
    private static string path;
    private StreamReader reader;
    //[SerializeField]
    public static string chapter;
    private string displayLine = "";
    public static bool reading = false;

    //[SerializeField] private SignalAsset signalToSend;
    //private SignalEmitter runtimeEmitter;


    // Start is called before the first frame update
    void Start()
    {

        //chapter = SignalManager.chapter;
        path = Application.streamingAssetsPath + "/Dialogue/Game_GB.txt";
        reader = new(path);

        //runtimeEmitter = ScriptableObject.CreateInstance<SignalEmitter>();
        //runtimeEmitter.asset = signalToSend;

    }


    public void ReadBlock() {
        
        string line = reader.ReadLine();
        if ((!line.Contains(chapter) && !reading) || line.Contains('#') || line.Length == 0)
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
                            yield return new WaitUntil(ReadNext); 
                            textAsset.text = "";
                        }

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

    }

    private bool ReadNext() 
    { return Input.GetKeyDown(KeyCode.S); }

    public void ReadStart() {

        reader = new(path);
        ReadBlock();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W) && !reading) {
            //reader = new(path);
            //ReadBlock();
        }

    }
}
