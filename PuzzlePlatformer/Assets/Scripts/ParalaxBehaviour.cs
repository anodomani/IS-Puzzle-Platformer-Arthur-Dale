using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBehaviour : MonoBehaviour
{
    public bool scaleWithCamera;
    private Vector2 dimensions;
    private Vector2 startPos;
    public float parallaxEffect;
    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        dimensions = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 temp = new Vector2(cam.transform.position.x, cam.transform.position.y) * parallaxEffect;

        transform.position = temp;
        if (temp.x > startPos.x + dimensions.x) { startPos += new Vector2(dimensions.x, 0); }
        else if (temp.x < startPos.x + dimensions.x) { startPos -= new Vector2(dimensions.x, 0); }

        if (temp.y > startPos.y + dimensions.y) { startPos += new Vector2(0, dimensions.y); }
        else if (temp.y < startPos.y + dimensions.y) { startPos -= new Vector2(0, dimensions.y); }

    }
}
