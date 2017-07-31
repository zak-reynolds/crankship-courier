using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightSpin : MonoBehaviour {

    public Vector3 spinVector = Vector3.up;
    public float speed = 360;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(spinVector * speed * Time.deltaTime);
	}
}
