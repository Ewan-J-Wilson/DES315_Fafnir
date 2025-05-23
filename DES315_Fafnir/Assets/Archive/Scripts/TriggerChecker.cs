using UnityEngine;

//Generic tool checker, exepected to be expanded upon for different movements in child classes
public class TriggerChecker : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tool"))
        { return; }
        DoAction();
    }

    protected virtual void DoAction() { }
}