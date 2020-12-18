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
    private bool _spawning = false;

    [SerializeField]
    private GameObject[] _powerups;

    [SerializeField]
    private GameObject[] _rarePowerups;

    private WaveManager _waveManager = null;

   

    // Start is called before the first frame update
    void Start()
    {
        _waveManager = GameObject.Find("Wave_Manager").GetComponent<WaveManager>();

        if (_waveManager == null)
        {
            Debug.LogError("Wave Manager is NULL in Spawn Manager Script.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeSpawning()
    {
        Debug.Log("started spawning");
        _spawning = true;
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRarePowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        //wait 1 frame
        yield return null;

        yield return new WaitForSeconds(3.0f);

        while (_spawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);

            //update enemy count in wave manager
            _waveManager.enemyCount++;

            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        
        while (_spawning)
        {
            //every 3 - 7 sec, spawn in a powerup
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, _powerups.Length);

            //Debug.Log("Random powerup: " + randomPowerup);
            
            Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
            
            yield return new WaitForSeconds(Random.Range(3.0f, 8.0f));
        }


    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
            while (_spawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomRarePowerup = Random.Range(0, _rarePowerups.Length);
            Instantiate(_rarePowerups[randomRarePowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 8.0f));
        } 
    }

    public void StartSpawning()
    {
        _spawning = true;
    }

    public void StopSpawning()
    {
        _spawning = false;
    }
}
