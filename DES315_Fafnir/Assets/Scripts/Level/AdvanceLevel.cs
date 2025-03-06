using NUnit.Framework.Internal.Commands;
using UnityEngine;

public class AdvanceLevel : MonoBehaviour
{

    [Tooltip("Reference to the Game Manager")]
    public GameManager gman;

    // Reference to the camera
    private Camera _camera;
    // Reference to the player
    private PlayerAI Player;

    private bool advanced = true;

    public void Awake() {

        Player = FindFirstObjectByType<PlayerAI>();
        _camera = GetComponent<Camera>();

    }

    public void Update() {

        float aspect = CameraScaler.targetAspect.x / CameraScaler.targetAspect.y;
        if (Player.transform.position.x >= _camera.orthographicSize * aspect 
            && !advanced && Time.timeScale != 0) {
            Debug.Log("Next");
            gman.NextLevel(); 
            advanced = true;
        }
        else if (advanced)
        { advanced = false; }

    }

}