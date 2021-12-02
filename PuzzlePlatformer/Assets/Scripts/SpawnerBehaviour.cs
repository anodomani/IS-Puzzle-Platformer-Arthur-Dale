using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    //generation variables
    public GameObject genObject;
    public int avatarAmount;
    public int avatarsSpawnedAtATime;
    public List<GameObject> avatars = new List<GameObject>();
    public List<MovementController> avatarsMovementControllers = new List<MovementController>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if there are less avatars than the chosen amount to spawn, spawns avatars
        if (avatars.Count < avatarAmount) 
        {
            populate(avatarsSpawnedAtATime);
        }
    }

    void populate(int timesToTrigger)
    {
        //spawn avatars
        for (int i = 0; i < timesToTrigger; i++)
        {
            float positionRand = Random.Range(-.1f, .1f);
            GameObject newG;
            newG = Instantiate(genObject, new Vector2(transform.position.x + positionRand, transform.position.y + positionRand), Quaternion.identity, transform);
            MovementController movementController = newG.GetComponent<MovementController>();
            avatars.Add(newG);
            avatarsMovementControllers.Add(movementController);
            PlayerManager.Instance.avatars.Add(newG);
            PlayerManager.Instance.avatarsMovementControllers.Add(movementController);
            newG.name = "Avatar " + avatars.Count;
        }
    }
}
