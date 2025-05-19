using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public void TeleportPlayer()
    {
        Transform teleportPoint = gameObject.GetComponent<TargetFinder>().FindBestTarget();

        if (teleportPoint.CompareTag("TeleportPoint"))
        {
            gameObject.transform.position = teleportPoint.transform.position;
        }
    }
}
