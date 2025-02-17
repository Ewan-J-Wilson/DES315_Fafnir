using UnityEngine;

public class AdvanceLevel : MonoBehaviour
{
    public GameManager gman;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) { gman.NextLevel(); }
    }
}