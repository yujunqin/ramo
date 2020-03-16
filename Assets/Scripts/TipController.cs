using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipController : MonoBehaviour
{
    public float timeActive;
    public float timeBetweenFlashes;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FlashSprite());
        StartCoroutine(DisableGameObject());
    }

    private IEnumerator FlashSprite()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenFlashes);
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
    }

    private IEnumerator DisableGameObject()
    {
        yield return new WaitForSeconds(timeActive);
        gameObject.SetActive(false);
    }
}
