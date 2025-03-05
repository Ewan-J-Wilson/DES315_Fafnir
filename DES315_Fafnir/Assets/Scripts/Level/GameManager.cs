
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

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
	private Audiomanager am;
	public PlayerAI Player;
	public Vector2 StartPos;
    public static GameState State;			//Current game state
	public static int LoopInd = 0;         //Current level/loop the player is on [one level is defined as a single loop]
    public GameObject[] LevelList;			//List of levels/loops in the scene
	[SerializeField]
	private AssetReference nextLevel;
	public int LevelInd;					//Current stage index
	private SpriteRenderer FadeOut;
	private bool doNextLevel = false;


    private void Start()
    {
		Player = FindFirstObjectByType<PlayerAI>();
		am = FindFirstObjectByType<Audiomanager>();
        State = GameState.Play;
        SetLevel();

		Camera _camera = FindFirstObjectByType<Camera>();		
		FadeOut = GameObject.FindGameObjectWithTag("FadeOut").GetComponent<SpriteRenderer>();
		FadeOut.transform.localScale = new(_camera.orthographicSize * _camera.aspect * 2, _camera.orthographicSize * 2);

		if (FadeOut.enabled)
		{ FadeOut.GetComponent<Fade>().FadeIn(); }
		else 
		{ FadeOut.enabled = false; }

    }

    //public void Awake() {
    //    
	//	
	//
    //}

    //Enable currentl level and disable other levels
    public void SetLevel()
	{

		//Debug.Log(LoopInd + "-" + LevelInd);
		

		if (LoopInd >= LevelList.Length) { 
			LoopInd = 0;
			doNextLevel = true;
			FadeOut.GetComponent<Fade>().FadeOut();
			return; 
		}
		else 
		{ am.FadeLoopTracks(LoopInd, LevelInd); }

		

		for (int i = 0; i < LevelList.Length; i++)
		{
			// Changed from LevelList[i].active so the editor shuts up
			LevelList[i].SetActive(i == LoopInd);
		}

		Player.PlayerDeath();
		
    }

    public void Update() {
        
		if (doNextLevel && FadeOut.GetComponent<Fade>().alpha >= FadeOut.GetComponent<Fade>().alphaThreshold) {
			doNextLevel = false;
			nextLevel.LoadSceneAsync(); 
		}

    }

    public void NextLevel()
	{
		foreach (TriggerGeneric trigger in FindObjectsByType<TriggerGeneric>(FindObjectsSortMode.None))
		{ trigger.Reset(); }

		if (LoopInd < LevelList.Length) { 
			LoopInd++;
		}

		SetLevel();
	}
}
