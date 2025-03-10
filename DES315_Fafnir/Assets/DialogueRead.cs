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
        if (!line.Contains(chapter))
        { 
            ReadBlock(); 
            return;
        }

        reader.Close();

        Debug.Log(line);
        displayLine = line;
        StartCoroutine(DisplayLine());

    }

    public IEnumerator DisplayLine() {

        TextMeshProUGUI textAsset = GetComponentInChildren<TextMeshProUGUI>();
        textAsset.text = "";

        foreach (char c in displayLine) {

            textAsset.text += c;
            yield return new WaitForSeconds(0.1f);
            
        }

    }


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W)) {
            reader = new(path);
            ReadBlock();
        }


    }
}
