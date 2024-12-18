using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UHealthComponent : MonoBehaviour
{
    [SerializeField]
    public float MAX = 50;
    public float Current;

    private float Percent => Current / MAX;
    private bool Dead;

    public System.Action<float> OnHealthModified;

    void Start()
    {
        Current = MAX;
    }

    public float Modify(float signedValue)
    {
        if (Dead) return Current;

        Current += signedValue;
        Current = Mathf.Clamp(Current, 0, MAX);

        if (Current <= 0)
        {
            Dead = true;
            Current = 0;
        }

        OnHealthModified?.Invoke(Percent);
        return Current;
    }
}
