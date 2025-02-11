using UnityEngine;

public class TriggerTest1 : TriggerChecker
{
    [SerializeReference] private GameObject door;
    protected override void DoAction()
    {
        door.SetActive(!door.activeSelf);
    }
}
