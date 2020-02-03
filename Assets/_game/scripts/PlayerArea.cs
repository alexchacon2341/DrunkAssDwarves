using UnityEngine;
using TMPro;

public class PlayerArea : MonoBehaviour
{
	public SpriteRenderer rune;
	public TextMeshPro scoreText;

	[HideInInspector]
	public PlayerController player;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Player" && player == other.GetComponent<PlayerController>() && player.gears.Count != 0)
		{
			for (int i = 0; i < player.gears.Count; i++)
			{
				if (player.gears[i].type == CollectibleTypes.Gear1)
				{
					player.score++;
				}

				Destroy(player.gears[i].gameObject);
			}

			scoreText.text = player.score.ToString();
			player.transform.position = transform.position;
			player.GoalReached();
		}
	}
}