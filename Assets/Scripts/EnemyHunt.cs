using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHunt : MonoBehaviour
{
    private RunTimeReferences runTimeReferences;
    Transform goal;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        runTimeReferences = GameObject.Find("Game Manager").GetComponent<RunTimeReferences>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = runTimeReferences.player.transform.position;
    }
}
