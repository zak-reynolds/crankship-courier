using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainDriver : MonoBehaviour {
    public enum CrankState { Engine, Gun, None };

    public CrankState State { get; private set; }

    [SerializeField]
    private float movementSpeed = 2;
    [SerializeField]
    private float maxLeft = -1.6f;
    [SerializeField]
    private float maxRight = 1.2f;

	void Start () {
        State = CrankState.Engine;
	}
	
	void Update () {
		if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition += Vector3.left * movementSpeed * Time.deltaTime;
            if (transform.localPosition.x < maxLeft)
            {
                transform.localPosition = Vector3.right * maxLeft;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.localPosition += Vector3.right * movementSpeed * Time.deltaTime;
            if (transform.localPosition.x > maxRight)
            {
                transform.localPosition = Vector3.right * maxRight;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("GunPoint")) {
            State = CrankState.Gun;
        }
        if (other.name.Equals("EnginePoint"))
        {
            State = CrankState.Engine;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("GunPoint") || other.name.Equals("EnginePoint"))
        {
            State = CrankState.None;
        }
    }

    public static CaptainDriver Get()
    {
        return GameObject.FindWithTag("Player").GetComponentInChildren<CaptainDriver>();
    }
}
