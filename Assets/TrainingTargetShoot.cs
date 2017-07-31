using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingTargetShoot : MonoBehaviour
{

    private float lifeTimer = 15;
    private Transform player;
    private bool sentMessage = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
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
            DialogueQueue.AddMessage("Gah! Shoot them, don't ram them you maniac!", 2);
            sentMessage = true;
            Doomed();
        }
        if (other.tag.Equals("PlayerBullet"))
        {
            DialogueQueue.AddMessage("Boom! Way to go!", 2);
            sentMessage = true;
            Doomed();
            DataDump.SetInt("TrainingTargetsShot", DataDump.GetInt("TrainingTargetsShot") + 1);
        }
    }

    public virtual void Doomed()
    {
        lifeTimer = 10;
        sentMessage = false;
        GobPool.Destroy(gameObject);
    }
}
