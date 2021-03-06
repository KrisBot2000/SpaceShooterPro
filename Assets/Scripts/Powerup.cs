﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3.0f;

    
    [SerializeField] // 0 = tripleshot, 1 = speed, 2 = shield, 3 = ammo
    private int _powerupID;

    private float _step = 0f;

    private Player _player = null;

    
    [SerializeField]
    private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        _step = _powerupSpeed * Time.deltaTime;

        _player = GameObject.Find("Player").GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            if (Input.GetKey(KeyCode.C))
            {
                //Debug.Log("C Key Down");
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _step);
            }
            else
            {
                //move down at speed of 3
                transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
            }
        }

        

        //when we leave the screen, destroy this object
        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    //OnTriggerCollision
    //Only be collectable by the Player (HINT: Use Tags)
    //on collected, destroy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //communicate with the player script via other
            //handle to the component I want
            //assign the handle to the component
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {

                switch (_powerupID)
                {
                    case 0:
                        //Debug.Log("tripleshot collected");
                        player.TripleShotActive();
                        break;
                    case 1:
                        //Debug.Log("speed boost collected");
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        //Debug.Log("shield collected");
                        player.ShieldsActive();
                        break;
                    case 3:
                        //Debug.Log("ammo collected");
                        player.AddAmmo();
                        break;
                    default:
                        Debug.Log("default case");
                        break;   
                }
            }

            AudioSource.PlayClipAtPoint(_clip, transform.position);
           
            Destroy(this.gameObject);
        }
    }   
}
