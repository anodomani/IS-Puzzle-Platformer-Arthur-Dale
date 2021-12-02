using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorBehaviour : MonoBehaviour
{
    public bool inverted;
    public Vector2 closedLength;
    public Vector2 openLength;
    public float opennessLerpRate;
    public float transitionLerpRate;
    public float inputDampener;
    public Activator activator;

    // Start is called before the first frame update
    void Start()
    {
        closedLength = transform.localScale;
        openLength = new Vector2(0, transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        opennessLerpRate = activator.output * inputDampener;
        if (inverted) { transform.localScale = Vector2.Lerp(transform.localScale, Vector2.Lerp(openLength, closedLength, opennessLerpRate), transitionLerpRate); }
        else { transform.localScale = Vector2.Lerp(transform.localScale, Vector2.Lerp(closedLength, openLength, opennessLerpRate), transitionLerpRate); }
    }
}
