using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner instance;
    public GameObject playerPrefab;
	public Transform tileGrid;
	public Transform guaranteedRocks;
	public Transform rockQuadrants;
	public PlayerArea[] playerAreas;
	public int removeRockRatio = 2;

	private List<PlayerArea> playerAreasList = new List<PlayerArea>();
	private List<PlayerController> players = new List<PlayerController>();

    void Awake()
    {
        instance = this;
    }

    public IEnumerator SpawnPlayersAsync()
    {
		GameManager.currentState = GameState.Loading;

		//Reset Grid Objects
		tileGrid.localScale = new Vector3(-tileGrid.localScale.x, tileGrid.localScale.y, tileGrid.localScale.z);

		for (int i = 0; i < guaranteedRocks.childCount; i++)
		{
			guaranteedRocks.GetChild(i).gameObject.SetActive(true);
		}

		for (int i = 0; i < rockQuadrants.childCount; i++)
		{
			List<GameObject> rocks = new List<GameObject>();

			for (int o = 0; o < rockQuadrants.GetChild(i).childCount; o++)
			{
				rockQuadrants.GetChild(i).GetChild(o).gameObject.SetActive(false);
				rocks.Add(rockQuadrants.GetChild(i).GetChild(o).gameObject);
			}

			for (int o = 0; o < rockQuadrants.GetChild(i).childCount / removeRockRatio; o++)
			{
				int random = Random.Range(0, rocks.Count);
				rocks[random].SetActive(true);
				rocks.RemoveAt(random);
			}
		}

		//Reset Players
		for (int i = 0; i < players.Count; i++)
		{
			players[i].gameObject.SetActive(false);
		}

		while(players.Count < ProjectReferences.instance.numberOfPlayers)
		{
			players.Add(Instantiate(playerPrefab).GetComponent<PlayerController>());
			players[players.Count - 1].playerID = players.Count - 1;
			players[players.Count - 1].gameObject.SetActive(false);

			yield return new WaitForEndOfFrame();
		}

		playerAreasList.Clear();

		for (int i = 0; i < playerAreas.Length; i++)
		{
			playerAreasList.Add(playerAreas[i]);
		}

		for (int i = 0; i < ProjectReferences.instance.numberOfPlayers; i++)
		{
			players[i].gameObject.SetActive(true);
			int random = Random.Range(0, playerAreasList.Count);
			EnablePlayerArea(playerAreasList[random], true, i);
			players[i].Reset(playerAreasList[random].transform.position);
			playerAreasList[random].player = players[i];
			playerAreasList.RemoveAt(random);
		}

		for (int i = 0; i < playerAreasList.Count; i++)
		{
			EnablePlayerArea(playerAreasList[i], false);
		}

		GameManager.currentState = GameState.Playing;
	}

	private void EnablePlayerArea(PlayerArea area, bool enable = true, int playerID = 0)
	{
		area.rune.color = ProjectReferences.instance.playerColors[playerID];
		area.GetComponent<SpriteRenderer>().enabled = enable;
		area.GetComponent<BoxCollider2D>().enabled = enable;
		area.scoreText.enabled = enable;
		area.rune.enabled = enable;
	}
}