using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LampBehaviour : MonoBehaviour
{
    public float flickerRate;
    public float flickerLevel;
    float[] normalLevel;

    public Light2D[] affectedLights;

    void Start()
    {
        normalLevel = new float[affectedLights.Length];
        for (int i = 0; i < affectedLights.Length; i++)
            {
                normalLevel[i] = affectedLights[i].intensity;
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0,100) < flickerRate )
        {
            for (int i = 0; i < affectedLights.Length; i++)
            {
                affectedLights[i].intensity = flickerLevel;
            }
        }
        else 
        {
           for (int i = 0; i < affectedLights.Length; i++)
            {
                affectedLights[i].intensity = normalLevel[i];
            } 
        }
    }
}
