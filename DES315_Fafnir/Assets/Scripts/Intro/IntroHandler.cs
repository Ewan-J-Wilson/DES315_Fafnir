using UnityEngine;

public class IntroHandler : MonoBehaviour
{
    public SpriteRenderer[] Panels;    //Panels to show
    [Tooltip("Amount of text lines until next panel")]
    public int[] TextCount;
    private int PanelInd;
    private int TextInd;
    
    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckPanels()
    {

    }
}
