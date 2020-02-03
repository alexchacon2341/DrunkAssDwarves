using UnityEngine;

public class ProjectReferences : MonoBehaviour
{
	public static ProjectReferences instance;
	public GameObject gear1Prefab;
	public GameObject deathEffect;
	public Color[] playerColors;

	//[HideInInspector]
	public int numberOfPlayers;

	private void Awake()
	{
		instance = this;
	}

	public string GetColorForPlayerID(int playerID)
	{
		switch (playerID)
		{
			case 0:
				return "Blue";
			case 1:
				return "Red";
			case 2:
				return "Green";
			case 3:
				return "Yellow";
		}

		return "";
	}
}