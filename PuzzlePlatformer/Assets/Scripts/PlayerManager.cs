using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //generation variables
    public GameObject genObject;
    public GameObject primaryObject;
    public int avatarAmount;
    public int avatarsSpawnedAtATime;
    public List<GameObject> avatars = new List<GameObject>();
    public GameObject primaryAvatarSpawned;

    //gameplay variables
    //public bool grounded;
    //public float groundedAvatarMin;

    public Vector3 primaryAdjustedPosition;
    public Vector3 autoAdjustedPosition;
    public bool mainAvatarRespawn = true;


    void Start()
    {
        //spawns in the primary avatar
        if (primaryObject != null && mainAvatarRespawn && PrimaryAvatarBehaviour.Instance == null)
        {
            GameObject newG;
            newG = Instantiate(primaryObject, new Vector2(transform.position.x, transform.position.y), Quaternion.identity, transform);
            avatars.Add(newG);
            newG.name = "Primary Avatar";
            primaryAvatarSpawned = newG;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if there are less avatars than the chosen amount to spawn, spawns avatars
        for (int i = 0; i < avatars.Count; i++) 
        {
            if (primaryObject != null && mainAvatarRespawn && PrimaryAvatarBehaviour.Instance == null)
            {
                GameObject newG;
                newG = Instantiate(primaryObject, new Vector2(transform.position.x, transform.position.y), Quaternion.identity, transform);
                avatars.Add(newG);
                newG.name = "Primary Avatar";
                primaryAvatarSpawned = newG;

                break;
            }
            if (avatars[i] == PrimaryAvatarBehaviour.Instance.primaryAvatar)
            {
                autoAdjustedPosition = avatars[i].transform.position;
                //primaryAdjustedPosition = AutoAdjustPosition();
                break;
            }
        }
        
        //spawn in avatars when there are less than the max ammount
        
        if (avatars.Count < avatarAmount) { populate(avatarsSpawnedAtATime); }
        if (Input.GetAxisRaw("QuickRespawn") != 0) { QuickRespawn(); }
        //CheckForGrounded();
    }

    void populate(int timesToTrigger)
    {
        //spawn avatars
        for (int i = 0; i < timesToTrigger; i++)
        {
            float positionRand = Random.Range(-.1f, .1f);

            GameObject newG;
            newG = Instantiate(genObject, new Vector2(transform.position.x + positionRand, transform.position.y + positionRand), Quaternion.identity, transform);
            avatars.Add(newG);
            newG.name = "Avatar " + avatars.Count;
        }
    }
    /*
    void CheckForGrounded()
    {
        int numberOfAvatarsGrounded = 0;
        grounded = false;
        print("grounded " + grounded);
        for (int i = 0; i < avatars.Count; i++)
        {
            if (avatars[i].GetComponent<PlayerController>().grounded)
            {
                numberOfAvatarsGrounded++;
            }
        }
        print("number of avatars grounded " + numberOfAvatarsGrounded);
        if (numberOfAvatarsGrounded > groundedAvatarMin)
        {
            grounded = true;
        }

        print("grounded 2 " + grounded);
    }
    */

    public void QuickRespawn()
    {
        //respawns an avatar and resets some aspects of it
        var moveToFirst = avatars[avatars.Count - 1];
        PlayerController playerController = moveToFirst.GetComponent<PlayerController>();
        //playerController.primaryByProxy = false;
        //playerController.primaryConnection = false;
        moveToFirst.transform.position = transform.position;
        avatars.RemoveAt(avatars.Count - 1);
        avatars.Insert(0, moveToFirst);
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
}
