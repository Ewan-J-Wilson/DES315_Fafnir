using NUnit.Framework.Internal.Commands;
using UnityEngine;

public class AdvanceLevel : MonoBehaviour
{

    [Tooltip("Reference to the Game Manager")]
    public GameManager gman;

    // Reference to the camera
    private Camera _camera;
    // Reference to the player
    private PlayerAI Player = null;

    public void Awake() {

        Player = FindFirstObjectByType<PlayerAI>();
        _camera = GetComponent<Camera>();

    }

    public void Update() {

        float aspect = CameraScaler.targetAspect.x / CameraScaler.targetAspect.y;
        if (Player == null)
        { return; }

        // If the player has reached the end of the camera
        if (Player.transform.position.x >= _camera.orthographicSize * aspect) {

            // Remove the player reference if they have reached the next level
            // to prevent multiple triggers of NextLevel
            if (GameManager.LoopInd >= gman.LevelList.Length - 1)
            { Player = null; }

            // Proceed to the next level
            gman.NextLevel(); 

        }

    }

}