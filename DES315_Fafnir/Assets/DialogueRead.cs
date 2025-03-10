using System.Collections;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System.Xml.Linq;

public class DialogueRead : MonoBehaviour
{
    
    public static float ReadSpeed = 0.02f;
    private static string path;
    private StreamReader reader;
    [SerializeField]
    private string chapter;
    private string displayLine = "";
    private bool reading = false;


    // Start is called before the first frame update
    void Start()
    {
        //path = Application.persistentDataPath + "/Dialogue/Game_GB.txt";
        path = "Assets/Dialogue/Game_GB.txt";

        reader = new(path);
        //ReadBlock();
    }


    public void ReadBlock() {
        
        string line = reader.ReadLine();
        if ((!line.Contains(chapter) && !reading) || line.Contains('#'))
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

        //Debug.Log(line);
        //displayLine = line;

        

    }

    public IEnumerator DisplayLine() {

        TextMeshProUGUI textAsset = GetComponentInChildren<TextMeshProUGUI>();
        textAsset.text = "";

        bool format = false;
        bool readIcon = false;
        string icon = "";

        foreach (char c in displayLine) {

            
            if (!(format || c == '[' || c == '{')) 
            { 

                textAsset.text += c;
                yield return new WaitForSeconds(ReadSpeed);
                //continue; 

            }
            else {

                if (format && (c == ']' || c == '}')) { 

                    if (c == ']') {

                        string iconPath = "Assets/Graphics/UI/Character/";
                        iconPath += icon.Split("_")[0] + "/";

                        Image character = GameObject.Find("Char Icon").GetComponent<Image>();
                        byte[] bytes = File.ReadAllBytes(Path.GetFullPath(iconPath + icon + ".png"));
                        character.sprite.texture.LoadImage(bytes);
                        

                    }

                    format = false;
                    readIcon = false;
                    // Clear the icon name
                    icon = "";
                }

                if (readIcon) {

                    icon += c;

                }

                if (c == '[') {

                    readIcon = true;
                    format = true;

                }

                if (c == '{')
                { format = true; }

                

            }


            
            
        }

        reading = false;

    }


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W) && !reading) {
            reader = new(path);
            ReadBlock();
        }


    }
}
