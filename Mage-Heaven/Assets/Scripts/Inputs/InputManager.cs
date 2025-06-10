using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Holds a reference to an InputAction and three UnityEvents
/// for started, performed, and canceled phases of the input.
/// </summary>
[Serializable]
public struct InputActionMapping
{
    public InputActionReference inputActionReference;
    public UnityEvent onStarted;
    public UnityEvent onPerformed;
    public UnityEvent onCanceled;
}

/// <summary>
/// and invokes corresponding UnityEvents in the Inspector.
/// </summary>
public class InputManager : MonoBehaviour
{
    [SerializeField] private List<InputActionMapping> inputActionMappings = new List<InputActionMapping>();

    // Stores all enabled actions so we can enable/disable them cleanly
    public List<InputAction> enabledActions = new List<InputAction>(); // Public for debugging

    private void Awake()
    {
        foreach (var mapping in inputActionMappings)
        {
            if (mapping.inputActionReference == null)
            {
                Debug.LogWarning("Input Action Reference is not assigned for one of the mappings.");
                continue;
            }

            InputAction action = mapping.inputActionReference.action;
            if (action == null)
            {
                Debug.LogWarning("Input Action is null for one of the mappings.");
                continue;
            }

            // Register handlers for all three input phases
            action.started += ctx => OnActionStarted(mapping);
            action.performed += ctx => OnActionPerformed(mapping);
            action.canceled += ctx => OnActionCanceled(mapping);

            enabledActions.Add(action);
        }
    }

    private void OnEnable()
    {
        // Enable all registered actions
        foreach (var action in enabledActions)
        {
            action.Enable();
        }
    }

    private void OnDisable()
    {
        // Disable all registered actions
        foreach (var action in enabledActions)
        {
            action.Disable();
        }
    }

    private void OnActionStarted(InputActionMapping mapping)
    {
        mapping.onStarted.Invoke();
    }

    private void OnActionPerformed(InputActionMapping mapping)
    {
        mapping.onPerformed.Invoke();
    }

    private void OnActionCanceled(InputActionMapping mapping)
    {
        mapping.onCanceled.Invoke();
    }
}
