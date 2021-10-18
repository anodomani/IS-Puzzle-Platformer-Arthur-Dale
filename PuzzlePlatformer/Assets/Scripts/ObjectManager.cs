using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject genObject;
    public int objectAmount;
    public int objectsSpawnedAtATime;

    public List<GameObject> objects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //spawn in avatars when there are less than the max ammount
        if (objects.Count < objectAmount)
        {
            populate(objectsSpawnedAtATime);
        }
    }

    void populate(int timesToTrigger)
    {
        for (int i = 0; i < timesToTrigger; i++)
        {
            float positionRand = Random.Range(-.1f, .1f);

            GameObject newG;
            newG = Instantiate(genObject, new Vector2(transform.position.x + positionRand, transform.position.y + positionRand), Quaternion.identity, transform);
            objects.Add(newG);
            newG.name = genObject.name + objects.Count;
        }
    }
}
