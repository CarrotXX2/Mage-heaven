using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleport : MonoBehaviour
{
    public void TeleportPlayer(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return; 
        
        Transform teleportPoint = gameObject.GetComponent<TargetFinder>().FindBestTarget();
        print("Tried to tp");
        
        if (teleportPoint.CompareTag("TeleportPoint"))
        {
            gameObject.transform.position = teleportPoint.transform.position;
        }
    }
}
