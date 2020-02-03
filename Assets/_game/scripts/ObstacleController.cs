using System.Collections;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject effectPrefab;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Pickaxe")
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other)
    {
		if (other.transform.tag == "Player")
        {
            StartCoroutine(PlayEffect(3f, other.contacts[0].point, other.gameObject));
        }
        else
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 0.1f);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator PlayEffect(float delay, Vector2 position, GameObject player)
    {
        var effect = (GameObject)Instantiate(effectPrefab, position, Quaternion.identity);
        Destroy(effect, 3f);
        //TODO: Stun pllayer? Tele back to start point? Drop any resources collected?
        yield return new WaitForSeconds(delay);
    }
}
