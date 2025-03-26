using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.Windows;

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

		DialogueManager.currentLevel = LevelInd;
		

	}

	private IEnumerator StartDialogue() {

		// Wait for the level transition fade to finish
		if (LoopInd >= LevelList.Length)
		{ 
			yield return new WaitUntil(_fade.IsFading); 
			yield return new WaitUntil(_fade.IsNotFading);
		}
		// Otherwise wait for the loop to load
		else
		{ yield return new WaitForSeconds(0.5f); }

		DialogueManager.OnLoopChange();

	}

    //Enable currentl level and disable other levels
    public void SetLevel()
    {

		if (GameObject.Find("Dialogue")) {
			DialogueManager.DisablePlayerInput(true);
			StartCoroutine(StartDialogue());
		}

		if (LoopInd >= LevelList.Length) { 
			LoopInd = 0;
			DialogueManager.currentLevel = LevelInd + 1;
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
		{ LevelList[i].SetActive(i == LoopInd); }

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
