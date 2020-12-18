using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;

    [SerializeField]
    private GameObject _explosionPrefab = null;

    private WaveManager _waveManager;



    private void Start()
    {
        _waveManager = GameObject.Find("Wave_Manager").GetComponent<WaveManager>();

        if(_waveManager == null)
        {
            Debug.LogError("Wave Manager is NULL in Asteroid script.");
        }
    }
    


    // Update is called once per frame
    void Update()
    {
        //rotate on zed axis
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    //check for LASER collision (Trigger)
    //instantiate collision at position of asteroid (us)
    //destroy explosion after 3 sec



    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "PlayerLaser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _waveManager.InitializeWaveSequence();
            Destroy(this.gameObject, 0.25f);
        }
    }
}
