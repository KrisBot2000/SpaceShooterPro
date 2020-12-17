using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public int wave;

    public int enemyKillCount;

    private int _enemyLimit;

    private SpawnManager _spawnManager;

    private bool _waveSequenceInitialized = false;



    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL in Wave Manager Script.");
        }
    }
     

    // Update is called once per frame
    void Update()
    {
        if (_waveSequenceInitialized == true && enemyKillCount >= _enemyLimit)
        {
            //stop spawning
            _spawnManager.StopSpawning();

            wave++;
            Debug.Log("Wave: " + wave);
            //display wave# for x amount of time

            //start spawning
            _spawnManager.StartSpawning();

            enemyKillCount = 0;
            _enemyLimit += 5;

        }
    }

    public void InitializeWaveSequence()
    {
        Debug.Log("wave sequence initialized");
        _waveSequenceInitialized = true;
        wave = 1;
        enemyKillCount = 0;
        _enemyLimit = 5;

        //display wave 1

        //start spawning
        _spawnManager.StartSpawning();
    }
}
