using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{

    public float respawnPoint = -50;
    public Vector3 size;
    float lerpRate = 0.1f;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        size = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -50)
        {
            Respawn();
        }

        if (transform.localScale.x < size.x || transform.localScale.y < size.y)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, size, lerpRate);
        }
    }

    void Respawn()
    {
        transform.localScale = new Vector3(0, 0, 0);
        rb.velocity = new Vector2(0, 0);
        transform.position = transform.parent.position;
    }
}
