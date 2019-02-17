using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private float _speed = 3.5f;

    //Explosion
    [SerializeField]
    private UnityEngine.GameObject _enemyExplosionPrefab;

    //UIManager
    private UIManager _uiManager;

    // Use this for initialization
    void Start()
    {
        _uiManager = UnityEngine.GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //move down
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //bring back to the screen coming down in a different, random position
        if (transform.position.y <= -6.0f)
        {
            transform.position = new Vector3(Random.Range(-7.8f, 7.8f), 6.0f, 0);
        }
    }

    //Collision detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        //check if the Enemy collided with the Laser
        if (other.tag == "Laser")
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(other.gameObject);

            _uiManager.UpdateScore();
        }
        //check if the Enemy collided with the Player
        else if (other.tag == "Player")
        {
            //access the player
            Player p = other.GetComponent<Player>();

            if (p != null)
            {
                p.Damage();
            }
        }

        //destroy the Enemy object
        Instantiate(_enemyExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}