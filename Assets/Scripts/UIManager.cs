using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //create handle to Text
    [SerializeField]
    private Text _scoreText = null;

    [SerializeField]
    private Image _LivesImg = null;

    [SerializeField]
    private Sprite[] _liveSprites = null;

    [SerializeField]
    private Text _ammoCountText = null;

    [SerializeField]
    private Text _ammoMaxText = null;

    [SerializeField]
    private Text _gameOverText = null;

    [SerializeField]
    private Text _restartText = null;

    //assign handle to Game Manager
    [SerializeField]
    private GameManager _gameManager = null;

    [SerializeField]
    private Player _player = null;

    [SerializeField]
    private Text _waveDisplayText = null;

    [SerializeField]
    private WaveManager _waveManager = null;

    
    




    // Start is called before the first frame update
    void Start()
    {
   
        _scoreText.text = "Score: " + 0;

        _ammoCountText.text = "Ammo: " + 0;

        _gameOverText.gameObject.SetActive(false);

        _waveDisplayText.gameObject.SetActive(false);

        

        //assign handle to component
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        //null check for GameManager
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL in UI Manager.");
        }
        else
        {
            _ammoMaxText.text = "/" + _player.ammoCountMax.ToString();
        }

        _waveManager = GameObject.Find("Wave_Manager").GetComponent<WaveManager>();

        if(_waveManager == null)
        {
            Debug.LogError("Wave Manager in UI Manager is NULL.");
        }

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives <= 0)
        {
            GameOverSequence();
        }
        else
        {
            _LivesImg.sprite = _liveSprites[currentLives];
        }  
    }

    public void UpdateAmmoCount(int playerAmmoCount)
    {
        _ammoCountText.text = "Ammo: " + playerAmmoCount.ToString();
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFickerRoutine());
    }

    IEnumerator GameOverFickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }   
    }

    public void PlayWaveDisplay(int wave)
    {
        //Debug.Log(wave.ToString());
        _waveDisplayText.gameObject.SetActive(true);
        //int waveString = wave.ToString();
        StartCoroutine(WaveDisplayRoutine(wave));
    }

    IEnumerator WaveDisplayRoutine(int wave)
    {
        _waveDisplayText.text = "WAVE " + wave;
        yield return new WaitForSeconds(0.4f);
        _waveDisplayText.text = "";
        yield return new WaitForSeconds(0.4f);
        _waveDisplayText.text = "WAVE " + wave;
        yield return new WaitForSeconds(0.4f);
        _waveDisplayText.text = "READY";
        yield return new WaitForSeconds(0.4f);
        _waveDisplayText.text = "GO!";
        yield return new WaitForSeconds(0.4f);
        _waveDisplayText.gameObject.SetActive(false);
    }

}

