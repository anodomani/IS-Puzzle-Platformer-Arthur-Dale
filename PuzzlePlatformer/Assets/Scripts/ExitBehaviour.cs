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

    private GameObject killTarget;

    void update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            reload = true;
            Load();
        }
        if (allowedToSwitchScenes)
        {
            Invoke("Load", delay);
        }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        AddToContents(other);
    }

    void AddToContents(Collider2D other)
    {
        //contents++;
        if (other.tag == "Player")
        {
            if (other.gameObject == PrimaryAvatarBehaviour.Instance.primaryAvatar)
            {
                textLevelDescription.text = levelCompleteText;
                PrimaryAvatarBehaviour.Instance.playerManager.mainAvatarRespawn = false;
                Load();
            }
            //PlayerController playerController = other.GetComponent<PlayerController>();
            //playerController.Respawn();
            killTarget = other.gameObject;
            Kill(other.gameObject);
        }
    }

    void Kill(GameObject other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (other == PrimaryAvatarBehaviour.Instance.primaryAvatar)
        {
            playerController.playerManager.mainAvatarRespawn = false;
            playerController.playerManager.avatars.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
        else
        {
            playerController.playerManager.QuickRespawn();
        }
    }

    public void AllowSceneSwitching()
    {
        allowedToSwitchScenes = true;
    }
}
