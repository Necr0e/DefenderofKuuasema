using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //Enemy Stats
    [SerializeField] float health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.1f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject deathSprite;
    int score = 100;
    bool runOnce = false;
    //References
    Transform target;
    GameObject gm;
    GameObject spawner;
    //Tracking code
    Vector3 shotDir;
    //Audio
    [SerializeField] public AudioClip enemyFireSound, enemyDeathSound;
    AudioSource audioSrc;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        gm = GameObject.Find("Game Manager");
        spawner = GameObject.Find("Enemy Spawner");
        audioSrc = GetComponent<AudioSource>();
    }
    void Update()
    {
        Aim();
    }

    private void Aim()
    {
        ShotCountdown();
        if(target != null)
        {
            shotDir = target.transform.position - transform.position;
        }

    }
    private void ShotCountdown()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots,maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        audioSrc.PlayOneShot(enemyFireSound);
        GameObject enemyProjectile = Instantiate(projectilePrefab,transform.position,Quaternion.AngleAxis(90,Vector3.back)) as GameObject;
        enemyProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(shotDir.x, shotDir.y * 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageTaken = other.gameObject.GetComponent<DamageDealer>();
        if(!damageTaken)
        {
            return;
        }

        ProcessHit(damageTaken);

    }

    private void ProcessHit(DamageDealer damageTaken)
    {

        health -= damageTaken.GetDamage();

        if(health <= 0)
        {
            gm.GetComponent<GameManager>().playerScore += score;
            spawner.GetComponent<EnemySpawner>().curNoEnemies--;
            if(!runOnce)
            {
                StartCoroutine(SpawnDeathSprite());
                runOnce = true;
            }
        }
    }

    IEnumerator SpawnDeathSprite()
    {
        audioSrc.PlayOneShot(enemyDeathSound);
        GameObject enemyDeath = Instantiate(deathSprite,transform.position,Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Destroy(enemyDeath);
        Destroy(gameObject);

    }
}
