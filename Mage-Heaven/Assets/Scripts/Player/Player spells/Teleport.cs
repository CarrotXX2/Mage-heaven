using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleport : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public void TeleportPlayer()
    {
        Transform teleportPoint = gameObject.GetComponent<TargetFinder>().FindBestTarget();
        print("Tried to tp");
        
        if (teleportPoint == null) return;
        
        if (teleportPoint.CompareTag("TeleportPoint"))
        {
            player.transform.position = teleportPoint.transform.position;
        }
    }
}
