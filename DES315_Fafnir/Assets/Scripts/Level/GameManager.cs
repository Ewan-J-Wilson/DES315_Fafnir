using UnityEngine;

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
	public static GameState State;			//Current game state
	public int LevelInd = 0;                //Current level/loop the player is on [one level is defined as a single loop]
    public GameObject[] LevelList;			//List of levels/loops in the scene


    private void Start()
    {
        State = GameState.Play;
        SetLevel();
    }

	//Enable currentl level and disable other levels
    public void SetLevel()
	{
		for (int i = 0; i < LevelList.Length; i++)
		{
			if (i == LevelInd) LevelList[i].active = true;
            else LevelList[i].active = false;
		}
	}
}
