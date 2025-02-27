using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	public GameObject Player;
	public Vector2 StartPos;
    public static GameState State;			//Current game state
	public static int LevelInd = 0;         //Current level/loop the player is on [one level is defined as a single loop]
    public GameObject[] LevelList;			//List of levels/loops in the scene
	[SerializeField]
	private SceneAsset nextLevel;
	public int StageInd;					//Current stage index


    private void Start()
    {
		am = FindFirstObjectByType<Audiomanager>();
        State = GameState.Play;
        SetLevel();
    }

	//Enable currentl level and disable other levels
    public void SetLevel()
	{

		if (LevelInd >= LevelList.Length) { 
			LevelInd = 0;
			SceneManager.LoadSceneAsync(nextLevel.name); 
			return;
		}

		for (int i = 0; i < LevelList.Length; i++)
		{
			// Changed from LevelList[i].active so the editor shuts up
			LevelList[i].SetActive(i == LevelInd);
		}

        Player.GetComponent<PlayerAI>().KillClone();
        Player.transform.position = StartPos;
		am.FadeLoopTracks(LevelInd, StageInd);
    }

	public void NextLevel()
	{
		foreach (TriggerGeneric trigger in FindObjectsByType<TriggerGeneric>(FindObjectsSortMode.None))
		{ trigger.Reset(); }
		LevelInd++;
		SetLevel();
	}
}
