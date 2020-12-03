using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab = null;

    [SerializeField]
    private GameObject _enemyContainer = null;

    [SerializeField]
    private bool _stopSpawning = false;

    [SerializeField]
    private GameObject[] _powerups;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnTripleShotPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //spawn game objects every 5 sec
    //create a coroutine of IEnumerator -- Yeild events
    //while loop

    IEnumerator SpawnEnemyRoutine()
    {
        //wait 1 frame
        yield return null;

        //then this line is called

        yield return new WaitForSeconds(3.0f);


        //then this line is called
        //while loop (infinite loop)
            //Instantiate enemy prefab
            //yeild wait for 5 sec

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnTripleShotPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            //every 3 - 7 sec, spawn in a powerup
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 4);
            //Debug.Log(randomPowerUp);
            Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 8.0f));
        }   

    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
