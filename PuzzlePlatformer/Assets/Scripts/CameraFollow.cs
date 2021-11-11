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
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        if (playerManager.avatars.Count > 0)
        {
            transform.position = Vector3.Lerp(transform.position, AutoAdjustPosition(), lerpRateMove);

            float cameraScale = FindFurthestAvatars();
            
            if (cameraScale > zoomMin) 
            {        
                if (cameraScale > zoomMax) { camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomMax, lerpRateZoom); }
                else { camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, cameraScale, lerpRateZoom); }
            }          
            else { camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomMin, lerpRateZoom); }
            
            camera.orthographicSize *= zoomScale;
            /*
            if (cameraScale > zoomMin) { camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, cameraScale, lerpRateZoom); }
            else { camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoomMin, lerpRateZoom); }
            */
        }
    }

    Vector3 AutoAdjustPosition()
    {
        float x = 0;
        float y = 0;
        for (int i = 0; i < playerManager.avatars.Count; i++)
        {
            x += playerManager.avatars[i].transform.position.x;
            y += playerManager.avatars[i].transform.position.y;
        }
        return new Vector3(x / playerManager.avatars.Count, y / playerManager.avatars.Count, transform.position.z);
    }

    float FindFurthestAvatars()
    {
        Vector3[] results = new Vector3[2];
        int highestPosition = 0;
        int lowestPosition = 0;

        for (int i = 0; i < playerManager.avatars.Count; i++)
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

        //(playerManager.avatars[highestPosition].transform.position.x + playerManager.avatars[highestPosition].transform.position.y + playerManager.avatars[lowestPosition].transform.position.x + playerManager.avatars[lowestPosition].transform.position.y) / 4;

        float returnedValue = Mathf.Max(
            (playerManager.avatars[highestPosition].transform.position.x - playerManager.avatars[lowestPosition].transform.position.x),
            (playerManager.avatars[highestPosition].transform.position.y - playerManager.avatars[lowestPosition].transform.position.y)
        );

        return returnedValue;
    }
}
