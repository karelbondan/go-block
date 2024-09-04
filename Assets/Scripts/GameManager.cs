using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] TMP_Text killCount;

    [Space]
    [Header("Prefab References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject damageTokenPrefab;

    [Space]
    [Header("Gameplay Related Variables")]
    // editable
    [SerializeField] private int killMilestone;

    [Space]
    [Header("UI Variables")]
    [SerializeField] private TMP_Text gameOverEnemiesKilledText;

    // non editable
    [HideInInspector] public int livingEnemies;
    private int enemiesKilled;
    private List<GameObject> Enemies = new List<GameObject>();
    

    private RunTimeReferences runTimeReferences;

    // Start is called before the first frame update
    void Start()
    {
        runTimeReferences = GetComponent<RunTimeReferences>();
        SpawnEnemies(waveManager.spawnAmount);
        enemiesKilled = 0;
        gameOverEnemiesKilledText.text = $"{enemiesKilled}";
    }

    // Update is called once per frame
    void Update()
    {
        if (livingEnemies == 1)
        {
            SpawnEnemies(5);
        }
        killCount.text = $"{enemiesKilled}";
    }

    public void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Enemies.Add(Instantiate(enemyPrefab, generateSpawnPoint(), Quaternion.identity));
        }
        livingEnemies += amount;
    }

    public void PurgeEnemies()
    {
        for(int i = 0; i < Enemies.Count; i++)
        {
           if (Enemies[i] != null) 
           {
                Enemies[i].GetComponentInChildren<EnemyManager>().DestroySelf();
           }
        }
        livingEnemies = 0;
        Enemies.Clear();
    }

    Vector3 generateSpawnPoint()
    {
        RaycastHit hit;
        Vector3 position = new Vector3(Random.Range(
            runTimeReferences.backLeft.transform.position.x, runTimeReferences.backRight.transform.position.x), 
            0, 
            Random.Range(runTimeReferences.backRight.transform.position.z, runTimeReferences.frontRight.transform.position.z
            )
        );

        while(true)
        {
            if (Physics.Raycast(position + new Vector3(0, 100.0f, 0), Vector3.down, out hit, 200.0f))
            {
                return hit.transform.position;
            }
        }
    }

    public void EnemiesKilled(Vector3 position, GameObject enemy)
    {
        enemiesKilled++;
        gameOverEnemiesKilledText.text = $"{enemiesKilled}";
        Enemies.Remove(enemy);
        if (enemiesKilled % (killMilestone + waveManager.killMilestoneModifier) == 0)
        {
            Instantiate(damageTokenPrefab, position, Quaternion.identity);
        }
    }
}
