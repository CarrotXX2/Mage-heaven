using System;
using UnityEngine;

public class TickTimer : MonoBehaviour
{
    public Action OnTick; // Delegate for listeners

    [SerializeField] private float tickRate; // Time it takes for each tick
    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= tickRate)
        {
            _timer = 0f;
            OnTick?.Invoke(); // Notify listeners
        }
    }
}
