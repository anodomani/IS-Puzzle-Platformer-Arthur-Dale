using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWellBehaviour : MonoBehaviour
{
    public float pull;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
            otherRb.velocity += new Vector2((transform.position.x - other.transform.position.x) * 15, (transform.position.y - other.transform.position.y)).normalized * pull;
            //otherRb.velocity += (Vector2)(other.transform.position - transform.position).normalized * (Vector2)(transform.position - other.transform.position);
        }
    }
}
