using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //allows us to view private variables in inspector
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private float _speedMultiplier = 2f;

    [SerializeField]
    private float _thrusterMultiplier = 1.05f;

    [SerializeField]
    private float _speedGovernor = 25.0f;


    [SerializeField]
    private GameObject _laserPrefab = null;

    [SerializeField]
    private GameObject _tripleShotPrefab = null;

    [SerializeField]
    private float _fireRate = 0.5f;

    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;


    private SpawnManager _spawnManager = null;

    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _isSpeedBoostActive = false;

    [SerializeField]
    private bool _isShieldsActive = false;

    [SerializeField]
    private GameObject _shieldsVisualizer;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private int _score;

    [SerializeField]
    private UIManager _uiManager;

    //variable to store audio clip
    [SerializeField]
    private AudioClip _laserSoundClip;

    private AudioSource _audioSource;




    // Start is called before the first frame update
    void Start()
    {
        //take current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if(_audioSource == null)
        {
            Debug.LogError("The AudioSource on Player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();

        Thrusters();

        //if I hit the space key
        //spawn gameObject

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }


    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -11f, 11f), 0);

        //horizontal movement
        //new vector3(1, 0, 0) * horizontalInput * speed * real time
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);


        //vertical movement
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        //if player position on y axis is greater than 0
        //reset y = 0
        //else if player position on y axis < -3.8
        //maintain y axis postion at -3.8

        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }

        else if (transform.position.y < -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        //BEST PRACTICE: transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        //This "clamps" the y-axis values between -3.8 and 0

        //if player position on x axis is > 11
        //wrap player position by x = -11
        //else if player position on x axis is < -11
        //wrap player position by x = 11

        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }

        else if (transform.position.x < -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            //call instantiate for triple shot
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }

        else
        {
            //Debug.Log("Space Key Pressed");
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {

        //if shields is active
        if (_isShieldsActive == true)
        {
            //deactivate shields
            _isShieldsActive = false;
            _shieldsVisualizer.SetActive(false);
            return;

        }
        
        //subtract 1 life
        _lives--;

        //update player animations
        UpdatePlayerAnimations();

        //upate lives
        _uiManager.UpdateLives(_lives);

        //check if dead
        //destroy us
        if (_lives < 1)
        {
            //communicate with spawn manager
            //let them know to stop spawning

            _spawnManager.OnPlayerDeath();

            Destroy(this.gameObject);
        }
        
    }

    //create method to modify triple shot:
    public void TripleShotActive()
    {
        //tripleShotActive becomes true
        _isTripleShotActive = true;
        //start the power-down coroutine for triple shot
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    //IEnumerator TripleShotPowerDownRoutine
    //wait 5 seconds
    //set the triple shot to false
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldsVisualizer.SetActive(true);
    }

    //method to add 10 to score
    //communicate with UI to update score

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    
    public void ReverseDamage()
    {
        if (_lives < 3)
        {
            //add 1 life
            _lives++;

            //update #lives on screen
            _uiManager.UpdateLives(_lives);

            //update player animations
            UpdatePlayerAnimations();


        }
    }

    public void UpdatePlayerAnimations()
    {
        switch (_lives)
        {
            case 3:
                _leftEngine.SetActive(false);
                _rightEngine.SetActive(false);
                break;
            case 2:
                _leftEngine.SetActive(true);
                _rightEngine.SetActive(false);
                break;
            case 1:
                _leftEngine.SetActive(true);
                _rightEngine.SetActive(true);
                break;
            default:
                Debug.Log("Player: UpdatePlayerAnimations default case.");
                break;
        }
    }
    

    void Thrusters()
    {
        //if LeftShift is pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //increases speed exponentially
            _speed *= _thrusterMultiplier;

            //limits speed to speed governor
            if (_speed > _speedGovernor)
            {
                _speed = _speedGovernor;
            }
        }
        //if LeftShift is de-pressed
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            //resets speed
            _speed = 3.5f;
        }
    }
}