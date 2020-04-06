using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyMovement : MonoBehaviour
{
    public float minX;
    public float maxX;
    public Sprite standingSprite;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool moving;
    private float timer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!moving)
        {
            moving = true;
            if (Random.value < 0.25)
            {
                StartCoroutine(Stand());
            }
            else
            {
                float xPosition = Random.Range(minX, maxX);
                spriteRenderer.flipX = (xPosition > transform.position.x);
                Vector3 newPosition = transform.position;
                newPosition.x = xPosition;
                float duration = Mathf.Abs(transform.position.x - xPosition) / 1;
                StartCoroutine(Move(transform, newPosition, duration));
            }
        }
    }

    private IEnumerator Stand()
    {
        animator.enabled = false;
        spriteRenderer.sprite = standingSprite;
        yield return new WaitForSeconds(3);
        moving = false;
        animator.enabled = true;
    }

    private IEnumerator Move(Transform from, Vector3 to, float duration)
    {
        float currentTime = 0;
        Vector3 start = from.position;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            from.position = Vector3.Lerp(start, to, currentTime / duration);
            yield return null;
        }
        
        moving = false;
    }
}
