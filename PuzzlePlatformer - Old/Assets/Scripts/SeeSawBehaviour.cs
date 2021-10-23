using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSawBehaviour : MonoBehaviour
{
    public Quaternion baseRotation;
    public float lerpRate;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float turnToBase = (baseRotation.z - transform.rotation.z) * 200;

        if (transform.rotation != baseRotation)
        {
            //float turnToBase = Mathf.Lerp(10, 20, 0.5f);
            rb.angularVelocity += turnToBase;
            //rb.rotation = turnToBase;
        }
    }
}
