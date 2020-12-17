using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    [SerializeField]
    private GameObject _laserPrefab = null;

    

    private Player _player;

    //create handle to animator component
    private Animator _anim;

    private AudioSource _audioSource;

    private float _fireRate = 3.0f;

    private float _canFire = -1;

    private WaveManager _waveManager;

    

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _audioSource = GetComponent<AudioSource>();

        _waveManager = GameObject.Find("Wave_Manager").GetComponent<WaveManager>();

        //null check player
        if(_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }
        if(_audioSource == null)
        {
            Debug.LogError("audio source in Player is NULL.");
        }
        if(_waveManager == null)
        {
            Debug.LogError("Wave Manager in Player is NULL");
        }

        //assign component to Anim
        _anim = GetComponent<Animator>();

        if(_anim == null)
        {
            Debug.LogError("The animator is NULL.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;

            //if enemy Collider2D exists
            if (GetComponent<Collider2D>() != null)
            {
                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }
            }
        }
    }


    void CalculateMovement()
    {

        //move down at 4 m/s
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        //if at bottom of screen
        //respawn at top -- extra challenge -- with a new random x position
        if (transform.position.y < -3.8f)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 7, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Hit: " + other.tag);

        //if other is Player
        //damage the player
        //destroy us

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            //trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;

            //play explosion sound
            _audioSource.Play();

            //destroy collider of enemy
            Destroy(GetComponent<Collider2D>());

            //update enemy kill counter
            _waveManager.enemyKillCount++;

            //destroy player
            Destroy(this.gameObject, 2.8f);
        }

        //if other is Laser
        //destroy laser
        //destroy us

        if (other.tag == "Laser" || other.tag == "HomingMissile")
        {
            //destroy laser or homing missile
            Destroy(other.gameObject);

            //destroy enemy collider
            Destroy(GetComponent<Collider2D>());

            //if player is still alive
            if (_player != null)
            {
                //increment score
                _player.AddScore(10);
            }

            //trigger anim
            _anim.SetTrigger("OnEnemyDeath");
           
            //stop movement
            //_enemySpeed = 0;

            //play explosion sound
            _audioSource.Play();

            //update enemy kill counter
            _waveManager.enemyKillCount++;

            //destroy enemy
            Destroy(this.gameObject, 2.8f);
            
        }
    }
}
