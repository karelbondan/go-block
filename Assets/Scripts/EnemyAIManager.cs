using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.Shapes;

public class EnemyAIManager : MonoBehaviour
{
    [Header("Hunt Settings")]
    [SerializeField] private float huntSpeed;
    [SerializeField] private float huntAngularSpeed;
    [SerializeField] private float huntAcceleration;

    [Header("Wander Settings")]
    [SerializeField] private float wanderSpeed;
    [SerializeField] private float wanderAngularSpeed;
    [SerializeField] private float wanderAcceleration;

    private EnemyWander wander;
    private EnemyHunt hunt;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wander = GetComponent<EnemyWander>();
        hunt = GetComponent<EnemyHunt>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            try
            {
                wander.enabled = false;
                hunt.enabled = true;
                agent.speed = huntSpeed; //7f;
                agent.angularSpeed = huntAngularSpeed; //250f;
                agent.acceleration = huntAcceleration; //20f;
            }
            catch
            {
                // haha biar g error anjing
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            wander.enabled = true;
            hunt.enabled = false;
            agent.speed = wanderSpeed; //3.5f;
            agent.angularSpeed = wanderAngularSpeed; //150f;
            agent.acceleration = wanderAcceleration; //8f;
        }
    }

    public IEnumerator disableMovement(float seconds)
    {
        wander.enabled = false;
        hunt.enabled = false;
        yield return new WaitForSeconds(seconds);
        hunt.enabled = true;
    }

    
}
