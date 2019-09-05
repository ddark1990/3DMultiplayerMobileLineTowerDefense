using UnityEngine;
using System.Collections;

public class SparkParticles : MonoBehaviour
{
    public Transform BillboardTarget;
    ParticleSystem _particleSystem;
    // There is a fixed buffer Size for particles hacked into this script.
    // This means the script will cap the max number of particles to whatever the length of this array is.
    // It's ugly I know. Sorry!
    ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[25];

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        if (BillboardTarget == null)
            BillboardTarget = Camera.main.transform;
    }

    void FixParticleRotations()
    {
        int count = _particleSystem.GetParticles(_particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 dir = _particles[i].velocity.normalized;
            float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _particles[i].rotation = rot;
        }

        _particleSystem.SetParticles(_particles, _particleSystem.particleCount);
    }

    public void Play()
    {
        transform.LookAt(BillboardTarget);
        _particleSystem.Play();
        StartCoroutine(LateFix());
    }

    // It seems setting the rotations on the frame you tell the system to Play is insufficient.
    // So this handy coroutine delays that by a single frame.
    IEnumerator LateFix()
    {
        yield return null;
        FixParticleRotations();
    }
}