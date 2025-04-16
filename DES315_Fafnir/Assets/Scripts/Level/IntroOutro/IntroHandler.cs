using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroHandler : MonoBehaviour
{
    public Fade fade;
    private GameObject DialogueObj;
    public SpriteRenderer[] Panels;    //Panels to show
    public int PanelInd;
    private SpriteRenderer PreviousPanel;
    private bool doNextScene = false;
    private bool loadingNext = false;
    
    private void Start()
    {
        
        PanelInd = -1;
        PreviousPanel = null;
    }

     
    // Update is called once per frame
    void Update()
    {

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
