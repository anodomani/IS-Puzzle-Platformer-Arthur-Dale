using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCubeBehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    public bool slowFall;
    public float slowFallAmount;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slowFall) { SlowFall(); }
    }

    void SlowFall()
    {
        if (rb.velocity.y < 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * slowFallAmount);
            //print("unkabunka");
        }
    }
}
