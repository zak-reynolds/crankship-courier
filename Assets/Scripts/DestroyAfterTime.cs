﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    public float timer = 5;
	
	void Update () {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject); // TODO: Pool?
        }
	}
}
