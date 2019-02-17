using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    //Player (Ship)
    [SerializeField]
    private float _speed = 10.0f;
    public int lives = 3;
    [SerializeField]
    private GameObject _playerExplosionPrefab;

    //Powerups states
    private bool _powerUpTripleShot = false;
    private int _tripleShotTime = 0;
    private bool _powerUpSpeedBoost = false;
    private int _speedBoostTime = 0;
    private bool _powerUpShield = false;
    private int _shieldTime = 0;

    //Laser
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    //Laser cool down system
    [SerializeField]
    private float _fireRate = 0.25f;
    private float _canFire = 0.0f;
    //Audio
    private AudioSource _laserSoundFX;

    //Shield
    [SerializeField]
    private GameObject _shieldGameObject;

    //Engine Failure
    [SerializeField]
    private GameObject _firstDamage;
    [SerializeField]
    private GameObject _secondDamage;

    //UIManager
    private UIManager _uiManager;
    //GameManager
    private GameManager _gameManager;
    //SpawnManager
    private SpawnManager _spawnManager;


    // Use this for initialization
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _laserSoundFX = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //enables player to move
        Movement();

    #if UNITY_ANDROID
        //fires a laser whenever the player presses the fire button
        if (CrossPlatformInputManager.GetButtonDown("Fire"))
        {
            Fire();
        }

    #elif UNITY_IOS
        //fires a laser whenever the player presses the fire button
        if (CrossPlatformInputManager.GetButtonDown("Fire"))
        {
            Fire();
        }
    #else
        //fires a laser whenever the player presses space or click the mouse left button
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            Fire();
        }
    #endif
    }

    //Custom methods

    //Lives System
    public void Damage()
    {
        //check if there's a shield enabled - if so, disable it
        if (_powerUpShield)
        {
            _powerUpShield = false;
            _shieldGameObject.SetActive(false);
            return;
        }

        lives--;
        _uiManager.UpdateLives(lives);

        //If it's player's last life, kill the Player
        if (lives == 2)
        {
            _firstDamage.SetActive(true);
        }
        else if (lives == 1)
        {
            _secondDamage.SetActive(true);
        }
        else if (lives < 1)
        {
            Instantiate(_playerExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            _gameManager.GameOver();
        }
    
    }


    //Moving the Player 1
    private void Movement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");

        //Moving 1.5x faster than normal speed (Speed Up Power Up)
        if (_powerUpSpeedBoost)
        {
            transform.Translate(Vector3.right * (_speed * 1.5f) * horizontalInput * Time.deltaTime); //move left-right 1.5x faster
            transform.Translate(Vector3.up * (_speed * 1.5f) * verticalInput * Time.deltaTime); //move up-down 1.5x faster
        }

        //Moving at normal speed
        else
        {
            transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime); //move left-right
            transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime); //move up-down
        }

        Wraping();
    }


    //Wraping
    void Wraping()
    {
        //On the y axis
        if (transform.position.y <= -4.2f)
        {
            transform.position = new Vector3(transform.position.x, -4.2f, 0);
        }
        else if (transform.position.y > 2f)
        {
            transform.position = new Vector3(transform.position.x, 2f, 0);
        }

        //On the x axis
        if (transform.position.x < -8.3f)
        {
            transform.position = new Vector3(-8.3f, transform.position.y, 0);
        }
        else if (transform.position.x > 8.3f)
        {
            transform.position = new Vector3(8.3f, transform.position.y, 0);
        }
    }


    //Firing Laser
    private void Fire()
    {
        if (Time.time > _canFire)
        {
            if (_gameManager.muteSoundFX == false)
            {
                //play firing Sound FX
                _laserSoundFX.Play();
            }

            //Firing Triple Shot 
            if (_powerUpTripleShot)
            {
                //spaw laser from right above the player position
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            //Firing normal single shot
            else
            {
                //spaw laser from right above the player position
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
            }

            //implementing the cool down system
            _canFire = Time.time + _fireRate;
        }
    }

    //Enable Triple Shot Powerup
    public void TripleShotPowerupOn()
    {
        _powerUpTripleShot = true;
        StopCoroutine("TripleShotPowerDownRoutine");
        _tripleShotTime += 5;
        StartCoroutine("TripleShotPowerDownRoutine");
    }
    //Disable Triple Shot Powerup
    public IEnumerator TripleShotPowerDownRoutine()
    {
        int temp = _tripleShotTime;
        for (int i = 0; i < temp; i++)
        {
            yield return new WaitForSeconds(1.0f);
            --_tripleShotTime;
        }
        _powerUpTripleShot = false;
    }

    //Enable Speed Boost Powerup
    public void SpeedUpPowerupOn()
    {
        _powerUpSpeedBoost = true;
        StopCoroutine("SpeedBoostPowerDownRoutine");
        _speedBoostTime += 5;
        StartCoroutine("SpeedBoostPowerDownRoutine");
    }
    //Disable Speed Boost Powerup
    public IEnumerator SpeedBoostPowerDownRoutine()
    {
        int temp = _speedBoostTime;
        for (int i=0; i < temp; i++)
        {
            yield return new WaitForSeconds(1.0f);
            --_speedBoostTime;
        }
        _powerUpSpeedBoost = false;
    }

    //Enable Shield Power Up
    public void ShieldPowerupOn()
    {
        _powerUpShield = true;
        StopCoroutine("ShieldPowerDownRoutine");
        _shieldGameObject.SetActive(true);
        _shieldTime += 10;
        StartCoroutine("ShieldPowerDownRoutine");
    }
    //Disable Shield Power Up after 10secs
    public IEnumerator ShieldPowerDownRoutine()
    {
        int temp = _shieldTime;
        for (int i=0; i < temp; i++)
        {
            yield return new WaitForSeconds(1.0f);
            --_shieldTime;
        }
        _powerUpShield = false;
        _shieldGameObject.SetActive(false);
    }

}