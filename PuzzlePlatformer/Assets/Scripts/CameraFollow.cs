using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerManager playerManager;
    public float lerpRateMove;
    public float lerpRateZoom;
    public float zoomMin;
    public float zoomMax;
    public float zoomScale;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        if (playerManager.avatars.Count > 0)
        {
            transform.position = Vector3.Lerp(transform.position, AutoAdjustPosition(), lerpRateMove);

            float cameraScale = FindFurthestAvatars();
            
            if (cameraScale > zoomMin) 
            {        
                if (cameraScale > zoomMax) { cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomMax, lerpRateZoom); }
                else { cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cameraScale, lerpRateZoom); }
            }          
            else { cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomMin, lerpRateZoom); }
            
            cam.orthographicSize *= zoomScale;
            /*
            if (cameraScale > zoomMin) { cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, cameraScale, lerpRateZoom); }
            else { cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomMin, lerpRateZoom); }
            */
        }
    }

    Vector3 AutoAdjustPosition()
    {
        int activeAvatars = 0;
        float x = 0;
        float y = 0;
        for (int i = 0; i < playerManager.avatars.Count; i++)
        {
            if (playerManager.avatarsMovementControllers[i].active)
            {
                x += playerManager.avatars[i].transform.position.x;
                y += playerManager.avatars[i].transform.position.y;
                activeAvatars++;
            }
        }
        return new Vector3(x / activeAvatars, y / activeAvatars, transform.position.z);
    }

    float FindFurthestAvatars()
    {
        Vector3[] results = new Vector3[2];
        int highestPosition = 0;
        int lowestPosition = 0;

        for (int i = 0; i < playerManager.avatars.Count; i++)
        {   
            if (playerManager.avatarsMovementControllers[i].active == true)
            {
                if ((playerManager.avatars[i].transform.position.x + playerManager.avatars[i].transform.position.y) > (playerManager.avatars[highestPosition].transform.position.x + playerManager.avatars[highestPosition].transform.position.y))
                {
                    highestPosition = i;
                }

                if ((playerManager.avatars[i].transform.position.x + playerManager.avatars[i].transform.position.y) < (playerManager.avatars[lowestPosition].transform.position.x + playerManager.avatars[lowestPosition].transform.position.y))
                {
                    lowestPosition = i;
                }
            }
        }

        //(playerManager.avatars[highestPosition].transform.position.x + playerManager.avatars[highestPosition].transform.position.y + playerManager.avatars[lowestPosition].transform.position.x + playerManager.avatars[lowestPosition].transform.position.y) / 4;

        float returnedValue = Mathf.Max(
            (playerManager.avatars[highestPosition].transform.position.x - playerManager.avatars[lowestPosition].transform.position.x),
            (playerManager.avatars[highestPosition].transform.position.y - playerManager.avatars[lowestPosition].transform.position.y)
        );

        return returnedValue;
    }
}
