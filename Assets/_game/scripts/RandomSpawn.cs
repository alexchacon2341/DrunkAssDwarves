using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
	public int oddsToSpawnNothing;
	public GameObject[] spawnPrefabs;
	public int[] spawnOdds;

	public void Spawn()
	{
		int oddsMax = 0;

		for (int i = 0; i < spawnOdds.Length; i++)
		{
			oddsMax += spawnOdds[i];
		}

		float random = Random.Range(0, oddsMax + oddsToSpawnNothing);

		if (random >= oddsMax)
		{
			return;
		}

		int prefabIndex = 0;

		for (int i = 0; i < spawnOdds.Length; i++)
		{
			if (random < spawnOdds[i])
			{
				prefabIndex = i;
			}
			else
			{
				break;
			}
		}

		if (spawnPrefabs[prefabIndex] != null)
		{
			GameObject spawn = Instantiate(spawnPrefabs[prefabIndex], transform.position, Quaternion.identity);
		}
	}
}