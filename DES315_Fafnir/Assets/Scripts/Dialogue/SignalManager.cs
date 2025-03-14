using System.Collections;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Timeline;

public class SignalManager : MonoBehaviour
{

    public static int currentLevel;
    public static int currentLoop;
    public static SignalManager instance;
    //public static string chapter;
    public DialogueRead textBox;

    public void Start() {
        
        instance = this;

        DontDestroyOnLoad(gameObject);

    }

    public static void OnLoopChange() {
    
        
    
    }
    
    public static void OnDiagObject(string _chapter) {
    
        Debug.Log("Signal Received at " + _chapter);
        DialogueRead.chapter = _chapter;
        instance.ReadDialogue();
    
    }

    public void ReadDialogue() {

        textBox.ReadStart();

    }


}