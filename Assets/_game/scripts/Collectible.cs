using System.Collections;
using UnityEngine;

public enum CollectibleTypes
{
	Gear1
}

public class Collectible : MonoBehaviour
{
	public static float resistance = 100;
	public CollectibleTypes type;
	public GameObject pickupEffectPrefab;
	public float minVelocity;
	public float maxVelocity;

	private Rigidbody2D rigid;
	private bool pickupCancel;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (pickupCancel)
		{
			return;
		}

		if (other.transform.tag == "Player")
		{
			if (type == CollectibleTypes.Gear1)
			{
				other.transform.GetComponent<PlayerController>().gears.Add(this);
				gameObject.SetActive(false);
			}

			if (pickupEffectPrefab)
			{
				GameObject effect = Instantiate(pickupEffectPrefab, other.contacts[0].point, Quaternion.identity, null);
				Destroy(effect, 1f);
			}
		}
	}

	public IEnumerator Fling ()
	{
		pickupCancel = true;
		Vector2 direction = new Vector2(Random.Range(-1,1),Random.Range(-1,1));
		rigid.velocity = direction * Random.Range(minVelocity, maxVelocity);

		while(Mathf.Abs(rigid.velocity.x) > 0.1f || Mathf.Abs(rigid.velocity.y) > 0.1f)
		{
			yield return new WaitForSeconds(0);

			float x = rigid.velocity.x;
			float y = rigid.velocity.y;

			if (x < 0)
			{
				x += resistance * Time.deltaTime;

				if (x > 0)
				{
					x = 0;
				}
			}
			else if (x > 0)
			{
				x -= resistance * Time.deltaTime;

				if (x < 0)
				{
					x = 0;
				}
			}

			if (y < 0)
			{
				y += resistance * Time.deltaTime;

				if (y > 0)
				{
					y = 0;
				}
			}
			else if (y > 0)
			{
				y -= resistance * Time.deltaTime;

				if (y < 0)
				{
					y = 0;
				}
			}

			rigid.velocity = new Vector2(x,y);
		}

		rigid.velocity = Vector2.zero;
		pickupCancel = false;
	}
}