using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class HapticFeedback : MonoBehaviour
{
    public static HapticFeedback Instance { get; private set; }

    public HapticImpulsePlayer leftController;
    public HapticImpulsePlayer rightController;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TriggerHaptic(XRBaseController controller, float amplitude, float duration)
    {
        if (controller != null)
            controller.SendHapticImpulse(amplitude, duration);
    }

    public void TriggerLeft(float amplitude, float duration)
    {
        leftController.SendHapticImpulse(amplitude, duration);
    }

    public void TriggerRight(float amplitude, float duration)
    {
        rightController.SendHapticImpulse(amplitude, duration);
    }
}
