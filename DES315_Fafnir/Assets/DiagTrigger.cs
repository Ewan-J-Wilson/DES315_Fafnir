using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements.Experimental;

public class DiagTrigger : MonoBehaviour
{

    [SerializeField] private SignalAsset signalToSend;
    private SignalEmitter runtimeEmitter;

    // Start is called before the first frame update
    void Start()
    {
        runtimeEmitter = ScriptableObject.CreateInstance<SignalEmitter>();
        runtimeEmitter.asset = signalToSend;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !DialogueRead.reading) {
            //reader = new(path);
            //ReadBlock();
            Debug.Log(runtimeEmitter.asset);
            SignalReceiver receiver = FindFirstObjectByType<SignalManager>().GetComponent<SignalReceiver>();
            receiver.OnNotify(default, runtimeEmitter, default);
        }
    }
}
