/********************************************************************************
*Creator(s)..........................................................Karim Dabboussi
*Created................................................................3/20/2019
*Last Modified................................................@ 5 PM on 04/19/2019
*Last Modified by....................................................Karim Dabboussi
*Description: A object/enemy spawner for spawning in different objects in unity.
*********************************************************************************
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemySpawn; // enemy to be selected to be spawned
    public bool stopSpawning = false;
    public bool randomSpawning; // a bool to be enabled in the editor if randomspawning for a spawn point is needed
    public bool infiniteEnemies;// only check if infinite enemies are needed
    public bool continousSpawning;// If you want the enemies to continue to spawn after reaching the limit
    public float spawnTime;
    public float spawnDelay;
    public float range; // how close the enemy has to be to spawn
    private int enemyCount = 0;
    private int start = 0;
    public int maxEnemies;// Max Enemies desired
    public Transform target;
    public float distance;
    List<GameObject> objects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
            InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }
    private void Update()
    {
        target = GameObject.Find("Yumi").transform; // Finds Yumi as the target
        distance = Vector3.Distance(this.transform.position, target.position);
        if (continousSpawning == true)
        {
            enemyCount = objects.Count;
            Debug.Log("List Count" + objects.Count);
            objects.Remove(null);
        }
        if (range <= distance)
        {
            stopSpawning = true;
        }
        else
        {
            stopSpawning = false;
        }
    }

    public void SpawnObject()
    {
        if (stopSpawning == false && enemyCount < maxEnemies) { 
            if (randomSpawning == true)
            {
                    Random random = new Random();
                    int randomNumber = Random.Range(0, 10);
                    if (infiniteEnemies == false)
                    {
                    if (continousSpawning == false)
                    {
                        enemyCount++;
                    }
                }
                    StartCoroutine(RandomTime(randomNumber));
            }
            else
            {

                if (infiniteEnemies == false)
                {
                    if (continousSpawning == false)
                    {
                        enemyCount++;
                    }
                }
                if (continousSpawning == true)
                {
                    objects.Add(Instantiate(enemySpawn, transform.position, transform.rotation));
                } else
                {
                    Instantiate(enemySpawn, transform.position, transform.rotation);
                }
                
            }
        }
    }

    public void EnemyKilled()
    {
        enemyCount--;
        Debug.Log("Enemy count is now " + enemyCount);
    }

    IEnumerator RandomTime(float num)
    {

        yield return new WaitForSeconds(num);

        if (continousSpawning == true)
        {
            objects.Add(Instantiate(enemySpawn, transform.position, transform.rotation));
        }
        else
        {
            Instantiate(enemySpawn, transform.position, transform.rotation);
        }
    }
    }
    
