using UnityEngine;
using UnityEngine.Timeline;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField] private string chapter;
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
            Debug.Log(runtimeEmitter.asset);
            SignalManager.chapter = chapter;
            SignalReceiver receiver = FindFirstObjectByType<SignalManager>().GetComponent<SignalReceiver>();
            receiver.OnNotify(default, runtimeEmitter, default);
        }
    }
}
