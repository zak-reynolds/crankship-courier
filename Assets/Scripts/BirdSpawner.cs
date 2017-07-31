using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject birdPrefab;
    [SerializeField]
    private GameObject burstPrefab;

    [SerializeField]
    private float rate = 1;

    [SerializeField]
    private float timer = 10;

    [SerializeField]
    private float burstTimer = 0;
    private float effectiveBurstTimer = 0;
    	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (birdPrefab && timer < 0)
        {
            SpawnBird();
            timer = rate;
        }

        if (burstPrefab)
        {
            UpdateBurst();
        }
    }

    void SpawnBird()
    {
        GameObject gob = GobPool.Instantiate(birdPrefab);
        gob.transform.SetPositionAndRotation(transform.position, transform.rotation);
        BirdDriver bd = gob.GetComponent<BirdDriver>();
        if (bd) bd.Start();
    }

    private void UpdateBurst()
    {
        effectiveBurstTimer -= Time.deltaTime;
        if (effectiveBurstTimer < 0)
        {
            GameObject gob = Instantiate(burstPrefab, transform.position + transform.forward * -20, transform.rotation); // TODO: Pool?
            SmoothTrack st = gob.GetComponent<SmoothTrack>();
            if (st)
            {
                st.target = transform;
            }
            effectiveBurstTimer = burstTimer;
        }
    }
}
