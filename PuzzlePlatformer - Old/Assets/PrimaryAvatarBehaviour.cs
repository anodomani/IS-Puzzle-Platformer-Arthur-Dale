using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryAvatarBehaviour : MonoBehaviour
{
    public static PrimaryAvatarBehaviour Instance { get; private set; }
    public GameObject primaryAvatar;
    public PlayerManager playerManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //print("singleton of GameControllerBehaviour lies at " + Instance.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerManager = transform.GetComponentInParent<PlayerManager>();
        primaryAvatar = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
