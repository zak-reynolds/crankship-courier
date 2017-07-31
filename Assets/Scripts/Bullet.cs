using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector3 Velocity { get; set; }
    public float wobble = 0;
    public float startTime;
    private float timer;
    private Vector3 realPosition;

    public void Start()
    {
        startTime = Time.time;
        timer = 1.2f;
        realPosition = transform.position;
    }

    void Update () {
        realPosition += Velocity * Time.deltaTime;
        transform.position = realPosition + (transform.up * wobble * Mathf.Sin((Time.time - startTime) * 50));
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            GobPool.Destroy(gameObject);
        }
	}
}
