using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingBay : MonoBehaviour
{

    public float speed = 5;
    public float wobble = 2;
    public float amplitude = 5;

    private Vector3 lastPosition;
    private Vector3 initialPosition;
    private float initialTime;

    protected Vector3 effectiveVelocity;
    [SerializeField]
    protected Vector3 startingVelocity;

    public virtual void Start()
    {
        initialPosition = transform.position;
        lastPosition = transform.position + Vector3.right;
        initialTime = Time.time;
        effectiveVelocity = startingVelocity;
    }

    void Update()
    {
        Move();
        transform.position += effectiveVelocity * Time.deltaTime;
        lastPosition = transform.position;
    }

    protected virtual void Move()
    {
        effectiveVelocity = new Vector3(
            speed,
            Mathf.Sin((Time.time - initialTime) * wobble) * amplitude,
            0);
    }
}
