using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToParentRotation : MonoBehaviour
{
    public float lerpRate = 0.1f;
    public MovementController movementController;
    public Vector3 rotationLeft;
    public Vector3 roatationRight;
    // Update is called once per frame
    bool facing = false;
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, transform.parent.position, lerpRate);
        if (movementController.h == 0) {}
        else if (movementController.h > 0.1f) { facing = true; }
        else if (movementController.h < 0.1f) { facing = false; }
        if (facing) { transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(roatationRight), lerpRate); }
        else { transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotationLeft), lerpRate); }
    }
}
