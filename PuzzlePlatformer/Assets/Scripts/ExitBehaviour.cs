using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitBehaviour : MonoBehaviour
{
    public List<GameObject> avatars = new List<GameObject>();
    public Text textLevelDescription;
    public string levelCompleteText;
    public float timeBeforeKill;
    public float delay;
    public string sceneName;
    public bool allowedToSwitchScenes;
    public bool reload = false;
    public LayerMask mask;

    private GameObject killTarget;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            reload = true;
            Load();
        }
    }

    void FixedUpdate()
    {
        RaycastHit2D playerHit;
        /*
        playerHit = Physics2D.CircleCast(transform.position, 2.5f, Vector2.down, 0.1f, mask.value);
        if (playerHit.collider != false) { print("YOWZA"); AddToContents(playerHit.collider.gameObject); }
        */
    }

    public void Load()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (!reload)
        {
            index += 1;
            if (index == SceneManager.sceneCountInBuildSettings) { index = 0; }
        }
        SceneManager.LoadScene(sceneName);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0, 0), 2.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AddToContents(other.gameObject);
    }

    void AddToContents(GameObject other)
    {
        MovementController movementController = other.GetComponent<MovementController>();

        if (movementController.primaryAvatar)
        {
            PlayerManager.Instance.avatars.Remove(other);
            PlayerManager.Instance.avatarsMovementControllers.Remove(other.GetComponent<MovementController>());
            //playerController.playerManager.avatars.Remove(other.gameObject);
            Destroy(other.gameObject);
            Load();
        }
        else
        {
            PlayerManager.Instance.QuickRespawn();
        }
    }
}
