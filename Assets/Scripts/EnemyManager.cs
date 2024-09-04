using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private float startingHealth = 6f;

    private float maxHealth;
    private GameManager gameManager;
    private WaveManager waveManager;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        waveManager = GameObject.Find("Game Manager").GetComponent<WaveManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        maxHealth = startingHealth + waveManager.enemyHealthModifier;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onShot(float damage)
    {
        float newHealth = currentHealth - damage;


        if (newHealth <= 0f)
        {
            gameManager.EnemiesKilled(transform.position, transform.parent.gameObject);
            gameManager.livingEnemies -= 1;
            Destroy(transform.parent.gameObject);
        }
        else
        {
            currentHealth = newHealth;
        }
    }

    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }
}
