using UnityEngine;

public enum GameState
{
	Loading,
	PlayerSelection,
	LevelIntro,
	InitializingGame,
	BuildPhase,
	Playing,
	GameEnd
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
	public static GameState currentState;
	public int[] cumulativeScores;

    void Awake()
    {
        instance = this;
		currentState = GameState.PlayerSelection;
		cumulativeScores = new int[ProjectReferences.instance.numberOfPlayers];
    }

    void Update()
    {
        if (currentState == GameState.PlayerSelection && Input.GetJoystickNames().Length < 2 && InputManager.Instance.KStart)
        {
			PlayerSpawner.instance.StartCoroutine(PlayerSpawner.instance.SpawnPlayersAsync());
        }
    }
}