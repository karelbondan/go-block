using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Properties")]
    public float startingWaveTime;
    public int startingEnemySpawn;

    // hidden properties only accessible by other scripts
    [HideInInspector] public int spawnAmount;
    [HideInInspector] public int damageModifier;
    [HideInInspector] public float enemyHealthModifier;
    [HideInInspector] public int currentWave;
    [HideInInspector] public int killMilestoneModifier;

    // wave manager spesific variables
    private int spawnModifier;
    private bool breakTime;

    [Space]
    [Header("accessed properties")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject NewWaveBanner;
    [SerializeField] private GameObject upperUI;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text waveIndicatorText;
    [SerializeField] private TMP_Text gameOverWavePassedText;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private AudioClip theZombiesAreComingSound;
    [SerializeField] private AudioClip nextWaveSound;

    private float WaveTimer;

    // Start is called before the first frame update
    void Awake()
    {
        currentWave = 1;
        spawnAmount = startingEnemySpawn;
        WaveTimer = startingWaveTime;
        damageModifier = 0;
        spawnModifier = 0;
        enemyHealthModifier = 0f;
        killMilestoneModifier = 0;

        // UI moment
        int minutes = Mathf.FloorToInt(WaveTimer / 60);
        int seconds = Mathf.FloorToInt(WaveTimer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // UI moment
        waveText.text = $"{currentWave}";
        gameOverWavePassedText.text = $"{currentWave - 1}";

        breakTime = false;
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!breakTime)
        {
            updateTimer();
        }
    }

    public void AdvanceWave()
    {
        currentWave++;
        // Every 1 waves make the gameManager to make the enemies harder to kill and more hard hitting
        if (currentWave % 1 == 0)
        {
            damageModifier++;
            enemyHealthModifier++;
        }

        // Every 2 waves make the gameManager to spawn extra enemies
        if (currentWave % 2 == 0)
        {
            spawnModifier += 2;
            killMilestoneModifier += 2;
        }

        waveText.text = $"{currentWave}";
        gameOverWavePassedText.text = $"{currentWave - 1}";
        gameManager.SpawnEnemies(startingEnemySpawn + spawnModifier);
        playerManager.modifyHealth(2.5f);
    }

    private void updateTimer()
    {
        if (WaveTimer > 0)
        {
            WaveTimer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(WaveTimer / 60);
            int seconds = Mathf.FloorToInt(WaveTimer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else if (WaveTimer <= 0)
        {
            timerText.text = "Break Time";
            WaveTimer = 0;
            waveIndicatorText.text = $"{currentWave + 1}";
            gameOverWavePassedText.text = $"{currentWave - 1}";
            breakTime = true;
            StartCoroutine(breakTimer());
            gameManager.PurgeEnemies();
        }

    }

    private IEnumerator breakTimer()
    {
        upperUI.LeanMoveLocalY(Screen.height, 0.7f).setEaseInSine().setIgnoreTimeScale(true);

        yield return new WaitForSeconds(3.5f);
        playerManager.PlayOneShotPlayer(nextWaveSound);
        NewWaveBanner.SetActive(true);
        playerManager.PlayOneShotPlayer(theZombiesAreComingSound);
        yield return new WaitForSeconds(1.5f);
        NewWaveBanner.GetComponent<WaveAnimation>().Hide();

        upperUI.LeanMoveLocalY(322f, 0.7f).setEaseOutExpo().setIgnoreTimeScale(true);
        
        breakTime = false;
        WaveTimer = startingWaveTime;
        AdvanceWave();
    }
}
