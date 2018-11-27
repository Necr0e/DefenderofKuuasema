using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int enemyCount = 3;
    [SerializeField] List<Transform> enemyWaypoints;
    [SerializeField] List<GameObject> enemyList;
    [SerializeField] public List<GameObject> enemyPos = new List<GameObject>();
    int waypointIndex;
    int enemyIndex;
    public int curNoEnemies = 0;
    float spawnDelay = 1.5f;
    float nextSpawn;
    GameObject player;
    GameObject enemies;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        SpawnEnemy();
        nextSpawn = Time.time + spawnDelay;
    }

    void PlayerVisibility()
    {
        if(!player.GetComponent<Player>().isVisible)
        {
            while(curNoEnemies >= 0)
            {
                RemoveEnemies();
            }

        }

        player.GetComponent<Player>().isVisible = true;
    }
    //TODO: code that makes sure enemies dont spawn on one another
    //TODO: make enemies despawn once you leave the screen, and respawn once you enter a new one.
    void Update()
    {
        //PlayerVisibility();   

        if(curNoEnemies <= 0)
        {
            enemyPos.Clear();
            Invoke("SpawnEnemy",5f);
        }
    }
    //If the current number of enemies on the field is less than Enemy count and spawn timer has not reacher 0, 
    void SpawnEnemy()
    {

        while (curNoEnemies <= enemyCount)
        {
            CreateEnemy();
            curNoEnemies++;
        }
    }

    //TODO: move enemies from spawning to an object pool
    void RemoveEnemies()
    {
        enemies = GameObject.FindWithTag("Enemy");
        Destroy(enemies);
        curNoEnemies = 0;
    }
    //TODO: Respawn delay
    //Randomly select a spawn location
    void SpawnLocation()
    {
        waypointIndex = Random.Range(0,7);
    }

    //Select random enemy to spawn, Spawn enemy.
    void CreateEnemy()
    {
        enemyIndex = Random.Range(0,2);         //Select a random enemy to spawn
        SpawnLocation();
        GameObject newEnemy = Instantiate(enemyList[enemyIndex],enemyWaypoints[waypointIndex].transform.position,Quaternion.AngleAxis(0f,Vector3.forward)) as GameObject;
        enemyPos.Add(newEnemy);                 //Save enemy positions to make sure they don't spawn ontop of one another
       // CheckSpawnOverlap();
    }

    //TODO: fix spawn overlap
    void CheckSpawnOverlap()
    {
        if(Vector3.Distance(enemyPos[0].transform.position,enemyPos[1].transform.position) == 0)
        {
            Debug.Log("Overlap detected");
            //SpawnLocation();
            enemyPos[1].transform.position = enemyWaypoints[waypointIndex].transform.position;
        }
    }

}
