using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatedPlatformBehaviour : MonoBehaviour
{
    public float speed;
    public Vector3 target1;
    public Vector3 target2;
    public bool movingDirection;

    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingDirection)
        {
            Vector3 direction = Vector3.zero;
            direction = target1 - transform.position;
            rb.AddRelativeForce(direction.normalized * speed);

            if (transform.position == target1)
            {
                movingDirection = false;
            }
        }
        if (!movingDirection)
        {
            Vector3 direction = Vector3.zero;
            direction = target2 - transform.position;
            rb.AddRelativeForce(direction.normalized * speed);

            if (transform.position == target2)
            {
                movingDirection = true;
            }
        }
    }
}
