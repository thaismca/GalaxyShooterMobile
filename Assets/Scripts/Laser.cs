using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField]
    private float _speed = 10;

	
	// Update is called once per frame
	void Update ()
    {
        //move up
        transform.Translate(Vector3.up * _speed * Time.deltaTime); 

        //Destroy the object when it gets completely out of the canvas
        if (transform.position.y >= 6.0f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
