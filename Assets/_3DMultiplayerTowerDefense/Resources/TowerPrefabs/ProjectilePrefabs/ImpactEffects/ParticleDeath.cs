using UnityEngine;

public class ParticleDeath : MonoBehaviour
{
    ParticleSystem pS;
    public float timer;

    private void Awake()
    {
        if(pS == null)
        {
            pS = GetComponent<ParticleSystem>();
            if(pS == null)
            {
                pS = GetComponentInChildren<ParticleSystem>();
            }
        }
        timer = pS.main.duration;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
