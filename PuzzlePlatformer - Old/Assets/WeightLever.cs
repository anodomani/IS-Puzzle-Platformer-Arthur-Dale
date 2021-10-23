using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightLever : MonoBehaviour
{
    public Vector2 basePosition;
    public float travelSpeed;
    public float currentSpeed;
    public float maxSpeed;
    public float currentPressure;
    public float dampenOvershootRate;
    Rigidbody2D rb;
    public Activator[] activator;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        basePosition = transform.position;
        rb.velocity += new Vector2(rb.velocity.x, Vector2.up.y * travelSpeed * 25);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < basePosition.y) 
        { 
            currentPressure = basePosition.y - transform.position.y; 
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += travelSpeed;
                
            }
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + (Vector2.up.y * currentSpeed));
        }
        else 
        {
            currentSpeed = 0;
            currentPressure = 0;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * dampenOvershootRate);
        }

        /*
        if (rb.velocity.magnitude < 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        */

        if (currentPressure < .1f) { currentPressure = 0; }
        
        for (int i = 0; i < activator.Length; i++)
        {
            activator[i].input = currentPressure;
        }
    }
}
