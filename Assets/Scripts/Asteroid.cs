﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;

    [SerializeField]
    private GameObject _explosionPrefab = null;

    private SpawnManager _spawnManager;



    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
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
        if(other.tag == "Laser")
        {

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.25f);
        }

    }





}
