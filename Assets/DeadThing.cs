using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadThing : MonoBehaviour {
    public Vector3 movement;
    public float deathShake = 1;

    public void Start()
    {
        var audioSource = GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.Play();
        }
        CameraDriver.SetSceenShake(deathShake);
    }
    void Update () {
        transform.position += movement * Time.deltaTime;
	}
}
