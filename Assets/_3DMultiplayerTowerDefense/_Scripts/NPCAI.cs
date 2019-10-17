using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public float goalDist;
    Transform goalPoint;

    void Start()
    {
        goalPoint = GameObject.FindGameObjectWithTag("GoalPoint").transform;
        agent.SetDestination(goalPoint.position);
    }

    void Update()
    {
        goalDist = Vector3.Distance(transform.position, goalPoint.position);
        if(goalDist <= 1)
        {
            Destroy(gameObject);
        }
    }
}
