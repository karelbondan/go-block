using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    [SerializeField]
    private GunSelector GunSelector;

    [SerializeField]
    private PlayerScriptableObject playerScriptableObject;

    [SerializeField]
    private float maxHealth = 3f;

    [SerializeField]
    private string enemyTag = "Enemy";

    // help
    [Header("Player Settings")]
    [SerializeField] private float stunnedSeconds;
    [SerializeField] private int stunnedMovementMultiplier;
    [SerializeField] private int stunnedSprintSpeed;
    [SerializeField] private int originalMovementMultiplier;
    [SerializeField] private int originalSprintSpeed;
    [SerializeField] Slider playerHealthBar;
    [SerializeField] GameObject damaged;
    [SerializeField] GameObject gameOver;
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioSource music;

    DamagedAnimation damagedAnimation;

    private float currentHealth;

    // modifier for powerups / debuff if we have time to implement all of this shit
    private float damageModifier = 0;

    private void Awake()
    {
        currentHealth = maxHealth;
        if (playerScriptableObject != null)
        {
            enemyTag = playerScriptableObject.EnemyTag;
            maxHealth = playerScriptableObject.BaseHealth;
        }

        damagedAnimation = damaged.GetComponent<DamagedAnimation>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gunHandler();
    }

    public void StunPlayer()
    {
        StartCoroutine(PlayerStunned());
    }

    private IEnumerator PlayerStunned()
    {
        MovePlayer properties = gameObject.GetComponent<MovePlayer>();
        properties.ChangeMovementSpeedMultiplier(stunnedMovementMultiplier, stunnedSprintSpeed); // 5, 5
        yield return new WaitForSeconds(stunnedSeconds); // 1 
        properties.ChangeMovementSpeedMultiplier(originalMovementMultiplier, originalSprintSpeed); // 20, 20
    }

    private void gunHandler()
    {
        // control for shooting gun
        if (!GunSelector.Auto)
        {
            if (Input.GetMouseButtonDown(0) && GunSelector.ActiveGun != null)
            {
                GunSelector.ActiveGun.Shoot();
            }
        }
        else if (GunSelector.Auto)
        {
            if (Input.GetMouseButton(0) && GunSelector.ActiveGun != null)
            {
                GunSelector.ActiveGun.Shoot();
            }
        }
    }

    public void handleShoot(RaycastHit hit, float damage)
    {
        float finalDamage = damage + damageModifier;
        if(hit.transform.tag == enemyTag)
        {
            // Get the enemy script
            EnemyManager enemyManager = hit.transform.GetComponent<EnemyManager>();
            if (enemyManager != null)
            {
                enemyManager.onShot(finalDamage);
            }
        }
    }

    public void modifyDamageModifier(float damage)
    {
        damageModifier += damage;
    }

    public void modifyMaxHealth(float health)
    {
        maxHealth += health;
    }

    public void modifyHealth(float health)
    {
        float newHealth = currentHealth + health;
        if (newHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = newHealth;
        }
        //playerHealthBar.value = currentHealth / maxHealth * 100;
        StartCoroutine(AnimateHealthBar());
    }

    public void onHit(float damage)
    {
        float newHealth = currentHealth - damage;
        if (newHealth <= 0)
        {
            currentHealth = 0;
            // testing purposes as always
            Debug.Log("You Died Lmao");
            
            // coroutine to ensure the damaged.setactive 
            // anim gets finished first
            StartCoroutine(Dead());
        }
        else
        {
            StartCoroutine(Damaged());
            currentHealth = newHealth;
        }
        //playerHealthBar.value = currentHealth / maxHealth * 100;
        StartCoroutine(AnimateHealthBar());
        soundSource.PlayOneShot(hitSound);
    }

    private IEnumerator AnimateHealthBar()
    {
        float lerpPos = 0;
        while (lerpPos <= 1f)
        {   
            playerHealthBar.value =  Mathf.Lerp(playerHealthBar.value, currentHealth / maxHealth * 100, lerpPos);
            lerpPos += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator Damaged()
    {
        damaged.SetActive(true);
        damagedAnimation.Activate();
        yield return new WaitForSeconds(0.1f);
        damagedAnimation.Hide();
    }

    private IEnumerator Dead()
    {

        soundSource.PlayOneShot(deathSound);
        music.volume = 0.5f;
        StartCoroutine(ChangeMusicVolumeBack());
        damaged.SetActive(true);
        damagedAnimation.Activate();
        yield return new WaitForSeconds(0.1f); // ensure above anim finish first
        gameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PlayGunShot()
    {
        soundSource.PlayOneShot(shootSound);
    }

    public void PlayOneShotPlayer(AudioClip audio)
    {
        soundSource.PlayOneShot(audio);
    }

    private IEnumerator ChangeMusicVolumeBack()
    {
        yield return new WaitForSecondsRealtime(deathSound.length);
        music.volume = 1f;
    }
}
