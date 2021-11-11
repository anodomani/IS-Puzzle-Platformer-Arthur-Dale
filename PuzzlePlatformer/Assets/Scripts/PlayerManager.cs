using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public List<GameObject> avatars = new List<GameObject>();
    public List<MovementController> avatarsMovementControllers = new List<MovementController>();
    public Vector3 autoAdjustedPosition;
    public bool mainAvatarRespawn = true;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {    

        if (Input.GetAxisRaw("QuickRespawn") != 0) { QuickRespawn(); }
        UpdateAvatars();
    }

    public void UpdateAvatars()
    {
        // input
        var h = Input.GetAxisRaw("Horizontal");
        var jump = Input.GetButtonDown("Jump");

        for (int i = 0; i < avatars.Count; i++)
        {
            //print(avatarsMovementControllers[i]);
            avatarsMovementControllers[i].Update_(h, jump);
        }
    }

    public Vector3 AutoAdjustPosition()
    {
        //calculates the auto adjust position of the avatars to the center of mass (not currently used)
        float x = 0;
        float y = 0;
        for (int i = 0; i < avatars.Count; i++)
        {
            x += avatars[i].transform.position.x;
            y += avatars[i].transform.position.y;
        }

        //print("auto adjusted pos = " + new Vector3(x / avatars.Count, y / avatars.Count, transform.position.z));
        return new Vector3(x / avatars.Count, y / avatars.Count, transform.position.z);
    }
    
    public void QuickRespawn()
    {
        //respawns an avatar at its home spawner
        var moveToFirst = avatars[avatars.Count - 1];
        moveToFirst.transform.position = moveToFirst.transform.parent.transform.position;
        avatars.RemoveAt(avatars.Count - 1);
        avatars.Insert(0, moveToFirst);
    }
}
