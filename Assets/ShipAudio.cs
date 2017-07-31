using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipDriver))]
public class ShipAudio : MonoBehaviour {

    [SerializeField]
    private AudioSource crankSource;
    [SerializeField]
    private AudioSource hpSource;
    [SerializeField]
    private AudioSource engineSource;
    [SerializeField]
    private AudioSource gunSource;
    [SerializeField]
    private AudioSource damageSource;

    [SerializeField]
    private AudioClip[] engineClips;
    [SerializeField]
    private AudioClip[] gunClips;

    private ShipDriver sd;
    private CaptainDriver cd;

    private float lastHP;
    
    void Start () {
        sd = GetComponent<ShipDriver>();
        sd.HPChanged.AddListener(OnHpChanged);
        lastHP = 5;
        cd = transform.GetComponentInChildren<CaptainDriver>();
        crankSource.volume = 0;
        crankSource.Play();
	}
	
	void Update () {
        int engineLevel = Mathf.Clamp(Mathf.FloorToInt(sd.GetEnginePowerNormalized() * 5), 0, 3);
        if (engineSource.clip != engineClips[engineLevel])
        {
            engineSource.clip = engineClips[engineLevel];
            engineSource.Play();
        }
        engineSource.volume = sd.GetEnginePowerNormalized();
        if (!sd.IsDocked())
        {
            engineSource.volume = Mathf.Max(0.25f, engineSource.volume);
        }

        int gunLevel = Mathf.Clamp(Mathf.FloorToInt(sd.GetGunPowerNormalized() * 4), 0, 2);
        if (gunSource.clip != gunClips[gunLevel])
        {
            gunSource.clip = gunClips[gunLevel];
            gunSource.Play();
        }
        gunSource.volume = sd.GetGunPowerNormalized();

        crankSource.volume = sd.IsCranking() ? 0.25f : 0;
    }

    void OnHpChanged(float newHP)
    {
        if (newHP > lastHP)
        {
            hpSource.Play();
        }
        else
        {
            damageSource.Play();
        }
        lastHP = newHP;
    }
}
