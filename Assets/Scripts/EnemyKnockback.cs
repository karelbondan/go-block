using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    [Header("Agent Settings")]
    [SerializeField] GameObject agent;
    [SerializeField] private float disableMovementSeconds;

    private EnemyAIManager agentManager;
    private WaveManager gameManager;

    private float damage;
    private float damageModifier;

    // Start is called before the first frame update
    void Start()
    {
        agentManager = agent.GetComponent<EnemyAIManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<WaveManager>();
        damage = 1;

        // this will be derived from a public variable in the game manager script later on
        damageModifier = gameManager.damageModifier;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerManager>().StunPlayer();
            StartCoroutine(agentManager.disableMovement(disableMovementSeconds)); // 2
            float finalDamage = damage + damageModifier;
            


            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            PlayerManager pm = other.gameObject.GetComponent<PlayerManager>();
            pm.onHit(finalDamage);
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            rb.AddForce(transform.up * 15.0f, ForceMode.Impulse);
            rb.AddForce(transform.forward * 15.0f, ForceMode.Impulse);
        }
    }
}
