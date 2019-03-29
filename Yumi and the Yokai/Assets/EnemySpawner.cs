using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemySpawn; // enemy to be selected to be spawned
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;
    public float range;
    private int enemyCount = 0;
    public int maxEnemies;
    public Transform target;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, Random.Range(1,10));
    }
    private void Update()
    {
        Debug.Log(enemyCount);
        target = GameObject.Find("Yumi").transform; // Finds Yumi as the target
        distance = Vector3.Distance(this.transform.position, target.position);
        if (range <= distance)
        {
            stopSpawning = true;
            Debug.Log("This line was invoked");
        }
        else
        {
            stopSpawning = false;
        }
    }

    public void SpawnObject()
    {
        if (stopSpawning == false && enemyCount < maxEnemies)
        {
            Random random = new Random();
            int randomNumber = Random.Range(0, 10);
            enemyCount++;
            Debug.Log(randomNumber);
            StartCoroutine(RandomTime(randomNumber));
            
        }
        // Update is called once per frame
    }

    public void EnemyKilled()
    {
        enemyCount--;
        Debug.Log("Enemy count is now " + enemyCount);
    }

    IEnumerator RandomTime(float num)
    {

        yield return new WaitForSeconds(num);
        Instantiate(enemySpawn, transform.position, transform.rotation);
    }
    
}
