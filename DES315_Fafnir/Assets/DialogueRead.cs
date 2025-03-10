using System.Collections;
using UnityEngine;
using System.IO;
using TMPro;

public class DialogueRead : MonoBehaviour
{
    
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
        if (!line.Contains(chapter) && !reading)
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

        if (line[^1] != ' ')
        { line += ' '; }
        displayLine += line;
        ReadBlock();

        //Debug.Log(line);
        //displayLine = line;

        

    }

    public IEnumerator DisplayLine() {

        TextMeshProUGUI textAsset = GetComponentInChildren<TextMeshProUGUI>();
        textAsset.text = "";

        foreach (char c in displayLine) {

            textAsset.text += c;
            yield return new WaitForSeconds(0.05f);
            
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
