using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerRenderer;

    [SerializeField] private Material SpriteDefaultMaterial;
    [SerializeField] private Material SpriteFlickMaterial;

    public IEnumerator StartFlick(float duration = 0.35f)
    {
        playerRenderer.material = SpriteFlickMaterial;

        yield return new WaitForSeconds(duration);

        playerRenderer.material = SpriteDefaultMaterial;
    }

#if UNITY_EDITOR
    //debug
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
            StartCoroutine(StartFlick());
    }
#endif
}
