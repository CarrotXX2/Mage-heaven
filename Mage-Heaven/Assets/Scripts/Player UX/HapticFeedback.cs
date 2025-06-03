using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticFeedback : MonoBehaviour
{
    public static HapticFeedback Instance { get; private set; }

    public XRBaseController leftController;
    public XRBaseController rightController;

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
        TriggerHaptic(leftController, amplitude, duration);
    }

    public void TriggerRight(float amplitude, float duration)
    {
        TriggerHaptic(rightController, amplitude, duration);
    }
}
