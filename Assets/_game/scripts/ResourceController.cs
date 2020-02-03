using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public GameObject pickupEffectPrefab;
    bool pickupInProgress;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (pickupInProgress)
            return;

        if (other.transform.tag == "Player")
        {
            pickupInProgress = true;
            StartCoroutine(PlayPickupEffect(3f, other.contacts[0].point, other.gameObject));
        }
    }

    private IEnumerator PlayPickupEffect(float delay, Vector2 position, GameObject player)
    {
        var effect = (GameObject)Instantiate(pickupEffectPrefab, position, Quaternion.identity);
        Destroy(effect, 1f);
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject, 0.1f);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //PlayerController pc = player.GetComponent<PlayerController>();
        //if(pc != null)
        //    pc.IncrementResourceCount();

        yield return new WaitForSeconds(delay);
    }
}
