using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdDriver : MonoBehaviour {

    public float speed = 5;
    public float wobble = 2;
    public float amplitude = 5;
    private float lifeTimer = 10;

    private Vector3 lastPosition;
    private Vector3 initialPosition;
    private float initialTime;

    protected Vector3 effectiveVelocity;
    [SerializeField]
    protected Vector3 startingVelocity;

    [SerializeField]
    private GameObject deadFacade;

    public virtual void Start() {
        initialPosition = transform.position;
        lastPosition = transform.position + Vector3.right;
        initialTime = Time.time;
        effectiveVelocity = startingVelocity;
    }

	void Update () {
        Move();
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0)
        {
            Doomed();
        }
        transform.rotation = Quaternion.FromToRotation(Vector3.left, effectiveVelocity.normalized);
        transform.position += effectiveVelocity * Time.deltaTime;
        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PlayerBullet"))
        {
            GobPool.Destroy(other.gameObject);
            DataDump.SetInt("BirdsKilled", DataDump.GetInt("BirdsKilled") + 1);
            var df = GobPool.Instantiate(deadFacade);
            df.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            Doomed();
        }
        if (other.tag.Equals("Player"))
        {
            DataDump.SetInt("BirdsKilled", DataDump.GetInt("BirdsKilled") + 1);
            other.GetComponent<ShipDriver>().TakeDamage(1);
            var df = GobPool.Instantiate(deadFacade);
            df.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            Doomed();
        }
    }

    protected virtual void Doomed()
    {
        lifeTimer = 10;
        GobPool.Destroy(gameObject);
    }

    protected virtual void Move()
    {
        effectiveVelocity = new Vector3(
            -speed,
            Mathf.Sin((Time.time - initialTime) * wobble) * amplitude,
            0);
    }
}
