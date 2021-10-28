using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    float h;
    string currentState;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // input
        h = Input.GetAxisRaw("Horizontal");

        // flip sprite
        if (Mathf.Abs(h) > 0.01f)
        {
            spriteRenderer.flipX = h > 0f;
        }
    }

    public void PlayAnimation(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        else
        {
            animator.Play(newState);

            currentState = newState;
        }
    }
}
