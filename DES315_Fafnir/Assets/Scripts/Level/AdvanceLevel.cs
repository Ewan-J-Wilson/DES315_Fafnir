using UnityEngine;

public class AdvanceLevel : MonoBehaviour
{
    public GameManager gman;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player") { gman.NextLevel(); }
    }
}