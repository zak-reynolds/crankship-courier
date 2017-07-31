using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDriver : MonoBehaviour {

    public float shakeAmount = 0;
    private static CameraDriver instance;

    private void Start()
    {
        instance = this;
    }

    void Update () {
		if (shakeAmount > 0)
        {
            transform.position += Vector3.right * Mathf.Sin(Time.time * 40) * shakeAmount;
            shakeAmount -= Time.deltaTime * 3;
        }
	}

    public static void SetSceenShake(float amount)
    {
        if (!instance)
        {
            instance = GameObject.FindWithTag("MainCamera").GetComponent<CameraDriver>();
            if (!instance) return;
        }
        if (amount > instance.shakeAmount)
        {
            instance.shakeAmount = amount;
        }
    }
}
