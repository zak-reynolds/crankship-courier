using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPickup : MonoBehaviour {

    private float lifeTimer = 10;
    
    void Start () {
		
	}
	
	void Update ()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer < 0)
        {
            Doomed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<ShipDriver>().AddHP(1);
            Doomed();
        }
    }

    protected virtual void Doomed()
    {
        lifeTimer = 10;
        GobPool.Destroy(gameObject);
    }
}
