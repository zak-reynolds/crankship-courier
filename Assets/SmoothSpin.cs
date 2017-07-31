using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothSpin : MonoBehaviour {

    public float speed;
    public float sinEffect;
    public float sinTime;

	void Start () {
		
	}
	
	void Update () {
        var updatedAxis = new Vector3(0, (Time.time * speed) + (Mathf.Sin(Time.time * sinTime) * sinEffect), 0);
        transform.rotation = Quaternion.Euler(updatedAxis);
	}
}
