using UnityEngine;

public class Rock : MonoBehaviour
{
	private RandomSpawn randomSpawn;

	private void Awake()
	{
		randomSpawn = GetComponent<RandomSpawn>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Pickaxe")
		{
			randomSpawn.Spawn();
			gameObject.SetActive(false);
		}
	}
}