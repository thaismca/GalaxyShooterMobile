using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour {

    private Animator _anim;

	// Use this for initialization
	void Start ()
    {
        _anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Turn Left when pressing Left Arrow key
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _anim.SetBool("turnLeft", true);
            _anim.SetBool("turnRight", false);
        }

        //Stop turning left and get back to idle when stop pressing Left Arrow key
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _anim.SetBool("turnLeft", false);
        }

        //Turn Right when pressing Right Arrow key
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
           _anim.SetBool("turnRight", true);
           _anim.SetBool("turnLeft", false);
        }

        //Stop turning right and get back to idle when stop pressing Right Arrow key
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
        _anim.SetBool("turnRight", false);
        }

    }
}
