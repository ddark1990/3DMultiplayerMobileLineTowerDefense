using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingVFX : MonoBehaviour
{
    public ParticleSystem[] effects;

    private void OnEnable()
    {
        foreach (ParticleSystem effect in effects)
        {
            effect.Play();
        }
    }
}
