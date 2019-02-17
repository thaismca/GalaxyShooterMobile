using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    [SerializeField]
    private float _scrollSpeed;
    [SerializeField]
    private float _tileSizeZ;

    private Vector3 _startPosition;

    //Game Mahager
    private GameManager _gameManager;

    

    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _startPosition = transform.position;
    }

    void Update()
    {
        if (_gameManager.gameOver == false)
        {
            float newPosition = Mathf.Repeat(Time.time * _scrollSpeed, _tileSizeZ);
            transform.position = _startPosition + Vector3.up * newPosition;
        }
        
    }
}
