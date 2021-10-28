using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fogBehaviour : MonoBehaviour
{

    public Vector3 basePosition;
    // Start is called before the first frame update
    void Start()
    {
        basePosition = transform.position;
        //transform.Rotate(Random.Range(-1.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector3.Lerp(transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.1f, 0.1f), 0), basePosition, 0.001f));
    }
}
