using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerBehaviour : MonoBehaviour
{
    public PlayerController playerController;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController playerControllerOther = other.GetComponent<PlayerController>();

            if (playerControllerOther)
            {
                if (playerControllerOther.grounded || playerControllerOther.groundedByProxy)
                {
                    playerController.groundedByProxy = true;
                    playerController.jumpTicksCurrent = playerController.jumpTicksMax;
                    print("ground me up scottie!");
                }
            }

        }
    }
}
