using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDriver : MonoBehaviour {

    public enum State { Wave, Bombers, Rest }

    private State state = State.Rest;
    public int bossHP = 100;//750;
    public float stateTimer = 15;
    private int hpSpawns = 10;

    private GameObject spawnWave;
    private GameObject spawnBomb;

    [SerializeField]
    private GameObject hpDrop;

    private void Start()
    {
        spawnWave = gameObject.transform.Find("ModeASpawner").gameObject;
        spawnBomb = gameObject.transform.Find("ModeBSpawner").gameObject;
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer < 0)
        {
            switch (state)
            {
                case State.Wave:
                    stateTimer = 15;
                    spawnWave.SetActive(true);
                    spawnBomb.SetActive(false);
                    state = State.Bombers;
                    break;
                case State.Bombers:
                    stateTimer = 15;
                    spawnWave.SetActive(false);
                    spawnBomb.SetActive(true);
                    state = State.Rest;
                    break;
                case State.Rest:
                    stateTimer = 10;
                    spawnWave.SetActive(false);
                    spawnBomb.SetActive(false);
                    state = State.Wave;
                    break;
            }
        }
        if (hpSpawns * 100 > bossHP)
        {
            hpSpawns--;
            var gob = GobPool.Instantiate(hpDrop);
            gob.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PlayerBullet"))
        {
            GobPool.Destroy(other.gameObject);
            bossHP--;
            DataDump.SetInt("BossHP", bossHP);
            if (bossHP < 0) Doomed();
        }
    }

    protected virtual void Doomed()
    {
        Destroy(gameObject);
    }
}
