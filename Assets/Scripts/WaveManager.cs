using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public int wave;

    public int enemyCount;

    private int _enemyLimit;

    private SpawnManager _spawnManager;

    private bool _waveSequenceInitialized = false;

    private UIManager _uIManager;



    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();


        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL in Wave Manager Script.");
        }
        if(_uIManager == null)
        {
            Debug.LogError("UI Manager in WaveManager is NULL.");
        }
    }
     

    // Update is called once per frame
    void Update()
    {
        if (_waveSequenceInitialized && enemyCount >= _enemyLimit)
        {
            //stop spawning
            _spawnManager.StopSpawning();

            //increment wave
            wave++;
            Debug.Log("Wave: " + wave);

            //display wave# for x amount of time
            _uIManager.PlayWaveDisplay(wave);

            //start spawning
            _spawnManager.StartSpawning();

            enemyCount = 0;
            _enemyLimit += 5;

        }
    }

    public void InitializeWaveSequence()
    {
        Debug.Log("wave sequence initialized");
        
        wave = 1;
        enemyCount = 0;
        _enemyLimit = 5;

        //display wave 1
        _uIManager.PlayWaveDisplay(wave);

        _waveSequenceInitialized = true;

        
        _spawnManager.InitializeSpawning();
    }
}
