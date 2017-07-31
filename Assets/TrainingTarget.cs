using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingTarget : MonoBehaviour
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
        if (!sentMessage && player.position.x > transform.position.x + 5)
        {
            sentMessage = true;
            DialogueQueue.AddMessage("Shucks, you missed that one. Try to hit the next target!", 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            DialogueQueue.AddMessage("Great hit!", 2);
            sentMessage = true;
            Doomed();
            DataDump.SetInt("TrainingTargetsHit", DataDump.GetInt("TrainingTargetsHit") + 1);
        }
    }

    public virtual void Doomed()
    {
        lifeTimer = 10;
        sentMessage = false;
        GobPool.Destroy(gameObject);
    }
}
