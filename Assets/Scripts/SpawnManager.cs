using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private GameObject _enemyShipPrefab;
    [SerializeField]
    private GameObject[] _powerups;

    //Game Manager
    private GameManager _gameManager;


	// Use this for initialization
	void Start ()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
 
    }

    public void StartSpawnRoutines()
    {
        StartCoroutine("SpawnEnemyShip");
        StartCoroutine("SpawnRandomPowerup");

    }

    public void StopSpawnRoutines()
    {
        StopCoroutine("SpawnEnemyShip");
        StopCoroutine("SpawnRandomPowerup");

    }

    //Coroutine to Spawn the Enemy every 3.0 secs
    public IEnumerator SpawnEnemyShip()
    {
        while (_gameManager.gameOver == false)
        {
            Instantiate(_enemyShipPrefab, transform.position = new Vector3(Random.Range(-7.8f, 7.8f), 6.0f, 0), Quaternion.identity);
            yield return new WaitForSeconds(3.0f);
        } 
    }

    //Coroutine to Spawn a Random Powerup every 5 secs
    public IEnumerator SpawnRandomPowerup()
    {
        while (_gameManager.gameOver == false)
        {
            int randomPowerup = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerup], transform.position = new Vector3(Random.Range(-7.8f, 7.8f), 6.0f, 0), Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }



}
