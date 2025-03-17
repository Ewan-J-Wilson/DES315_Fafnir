using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

//Current state the game is in, used to update different sections of code
public enum GameState
{
	Play,               //Game is currently being played by the player
	Cutscene,           //Cutscene is playing
	Load,               //Game is loading/transitioning
	Die,                //Player has died
}

public class GameManager : MonoBehaviour
{
	private static Audiomanager am;
	[HideInInspector]
	public PlayerAI Player;
    public static GameState State;			//Current game state
	public static int LoopInd = 0;         //Current level/loop the player is on [one level is defined as a single loop]
    public GameObject[] LevelList;			//List of levels/loops in the scene
	[SerializeField]
	private string nextLevel;
	public int LevelInd;					//Current stage index
	[HideInInspector]
	public static Fade _fade;
	private bool doNextLevel = false;
	// Can't set signalToSend as static due to needing to reference the asset in the inspector
	[SerializeField] private SignalAsset signalToSend;
	private SignalEmitter runtimeEmitter;


    private void Start()
    {
		
		Player = FindFirstObjectByType<PlayerAI>();
		if (am == null) 
		{ am = FindFirstObjectByType<Audiomanager>(); }
        State = GameState.Play;
        SetLevel();
		
    }

	private void Awake()
	{ 
		Camera _camera = FindFirstObjectByType<Camera>();
		_fade = GameObject.FindGameObjectWithTag("FadeOut").GetComponent<Fade>();
        float aspect = CameraScaler.targetAspect.x / CameraScaler.targetAspect.y;
        _fade.transform.localScale = new(_camera.orthographicSize * aspect * 2, _camera.orthographicSize * 2);

		SignalManager.currentLevel = LevelInd;
		SignalReceiver receiver = FindFirstObjectByType<SignalManager>().GetComponent<SignalReceiver>();
        receiver.OnNotify(default, runtimeEmitter, default);

	}

    //Enable currentl level and disable other levels
    public void SetLevel()
	{

		if (LoopInd >= LevelList.Length) { 
			LoopInd = 0;
			SignalManager.currentLevel = LevelInd + 1;
			doNextLevel = true;
			Time.timeScale = 0;
			_fade.FadeOut();
			return; 
		}
		else 
		{ 
			Player.PlayerDeath();
			am.FadeLoopTracks(LoopInd, LevelInd); 
		}

		for (int i = 0; i < LevelList.Length; i++)
		{
			// Changed from LevelList[i].active so the editor shuts up
			LevelList[i].SetActive(i == LoopInd);
		}

    }

    public void Update() {
        
		if (doNextLevel && _fade.alpha >= 1f) {
			doNextLevel = false;
			Time.timeScale = 1;
			SceneManager.LoadSceneAsync(nextLevel);
		}

    }

    public void NextLevel()
	{

		foreach (TriggerGeneric trigger in FindObjectsByType<TriggerGeneric>(FindObjectsSortMode.None))
		{ trigger.Reset(); }

		if (LoopInd < LevelList.Length) 
		{ LoopInd++; }
		
		SetLevel();
	}
}
