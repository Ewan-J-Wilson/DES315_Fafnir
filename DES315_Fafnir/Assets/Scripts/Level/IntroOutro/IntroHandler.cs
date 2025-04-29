using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroHandler : MonoBehaviour
{
    public Fade fade;
    private GameObject DialogueObj;
    public RawImage[] Panels;    //Panels to show
    public int PanelInd;
    private RawImage PreviousPanel;
    private bool doNextScene = false;
    private bool loadingNext = false;
    private bool skipHeld = false;
    [SerializeField]
    private float skipCooldown = 1f;
    private float skipTimer = 0f;
    
    private void Start()
    {
        
        PanelInd = -1;
        PreviousPanel = null;
    }

     
    public void SkipAction(InputAction.CallbackContext obj) {

        if (obj.performed)
        { skipHeld = true; }

        if (obj.canceled) {
            skipHeld = false;
            skipTimer = 0f;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (skipHeld) {
            skipTimer += Time.deltaTime;
            if (skipTimer >= skipCooldown) { 
                PanelInd = 7; 
                DialogueManager.ForceQuitDialogue();
            }
        }

        if (DialogueObj == null)
        { DialogueObj = FindFirstObjectByType<DialogueManager>().gameObject; }

        if (!DialogueRead.reading && PanelInd <= 5) { SetPanels(); }
        else if (PanelInd >= 6 && fade.IsNotFading())
        {
            fade.FadeOut();
            Time.timeScale = 0;
            doNextScene = true;
        }
        if (Fade.alpha >= 1 && doNextScene && !loadingNext) {

            loadingNext = true;
            Time.timeScale = 1;
            GameManager.LoopInd = 0;
            SceneManager.LoadSceneAsync("Level 1"); 
        
        }
        if (PreviousPanel != null) { PreviousPanel.color -= new Color(0, 0, 0, 2f) * Time.deltaTime; }
    }
    void SetPanels()
    {
        if (PanelInd != -1) PreviousPanel = Panels[PanelInd];
        PanelInd++;
        DialogueManager.CodedDialogue("INTRO_" + PanelInd);
    }
}
