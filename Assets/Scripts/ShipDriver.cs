using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipDriver : MonoBehaviour {

    [SerializeField]
    private float enginePower = 0;
    [SerializeField]
    private float gunPower = 0;
    [SerializeField]
    private float maxEnginePower = 1;
    [SerializeField]
    private float maxGunPower = 1;
    [SerializeField]
    private float engineDecay = 1;
    [SerializeField]
    private float gunDecay = 1;
    [SerializeField]
    private int hp = 5;
    [SerializeField]
    private int maxHP = 5;
    [SerializeField]
    private float gunWaveThreshold = 0.5f;
    [SerializeField]
    private float gunSpreadThreshold = 0.75f;
    public float unDockPowerThreshold = 0.75f;
    [SerializeField]
    private bool invincible = false;

    [SerializeField]
    private Transform gunBarrel;
    [SerializeField]
    private CaptainDriver captain;

    [SerializeField]
    private GameObject bulletPrefab;

    public UnityEvent<float> EnginePowerChanged;
    public UnityEvent<float> GunPowerChanged;
    public UnityEvent<float> HPChanged;

    private float effectiveEnginePower = 0;
    private float effectiveEngineDecay = 1;
    private float effectiveGunPower = 0;
    private float effectiveGunDecay = 0.2f;
    private Vector3 velocity = Vector3.zero;
    public float gunTimer = 0;
    private bool inSpace = false;
    [SerializeField]
    private bool docked = true;
    private float lastCrankTime = 0;

	public class PowerUpdateEvent : UnityEvent<float> { }

	void Awake () {
        EnginePowerChanged = new PowerUpdateEvent();
        GunPowerChanged = new PowerUpdateEvent();
        HPChanged = new PowerUpdateEvent();
    }
	
	// Update is called once per frame
	void Update () {
        effectiveEngineDecay = inSpace ? engineDecay * 6 : engineDecay;
        enginePower = Mathf.Max(0, enginePower - effectiveEngineDecay * Time.deltaTime);
        effectiveEnginePower = Mathf.Lerp(effectiveEnginePower, enginePower, 5 * Time.deltaTime);

        effectiveGunDecay = gunDecay;
        gunPower = Mathf.Max(0, gunPower - effectiveGunDecay * Time.deltaTime);
        effectiveGunPower = Mathf.Lerp(effectiveGunPower, gunPower, 5 * Time.deltaTime);

        velocity = new Vector3(
            effectiveEnginePower + 2f,
            effectiveEnginePower - 5f,
            0);

        if (!docked)
        {
            transform.position += velocity * Time.deltaTime;
            transform.rotation = Quaternion.FromToRotation(Vector3.right, velocity);
        }
        else
        {
            docked = GetEnginePowerNormalized() < unDockPowerThreshold;
            if (velocity.y > 0) transform.rotation = Quaternion.FromToRotation(Vector3.right, velocity);
        }

        gunTimer = Mathf.Max(0, gunTimer - Time.deltaTime);
        if (gunPower > 0 && gunTimer <= 0)
        {
            ShootBullet();
            gunTimer = 1 / (effectiveGunPower + 1);
        }

        EnginePowerChanged.Invoke(GetEnginePowerNormalized());
        GunPowerChanged.Invoke(GetGunPowerNormalized());
	}

    public void CrankEngine(float amount)
    {
        if (captain.State == CaptainDriver.CrankState.Engine && !inSpace)
        {
            enginePower = Mathf.Min(maxEnginePower, enginePower + amount);
            lastCrankTime = Time.time;
        }
    }

    public void CrankGun(float amount)
    {
        if (captain.State == CaptainDriver.CrankState.Gun)
        {
            gunPower = Mathf.Min(maxGunPower, gunPower + amount);
            gunTimer = 0;
            lastCrankTime = Time.time;
        }
    }

    public void TakeDamage(int amount)
    {
        hp = Mathf.Max(0, hp - amount);
        if (!invincible && hp <= 0)
        {
            Doomed();
        }
        HPChanged.Invoke((float)hp / maxHP);
    }

    public void AddHP(int amount)
    {
        hp = Mathf.Min(maxHP, hp + amount);
        HPChanged.Invoke((float)hp / maxHP);
    }

    private void Doomed()
    {
        DialogueQueue.AddMessage("Welp, you tried. Hit BACKSPACE to give it another shot (buh dum tiss).");
        Destroy(gameObject);
    }

    private void DockingSequence()
    {
        Destroy(gameObject);
    }

    private void ShootBullet()
    {
        GameObject b = GobPool.Instantiate(bulletPrefab);
        Bullet bc = b.GetComponent<Bullet>();
        b.transform.SetPositionAndRotation(gunBarrel.position, gunBarrel.rotation);
        bc.Start();
        bc.Velocity = gunBarrel.forward * 30 + velocity;
        bc.wobble = 0;
        if (gunPower / maxGunPower > gunWaveThreshold)
        {
            bc.wobble = 0.5f;
        }
        if (gunPower / maxGunPower > gunSpreadThreshold)
        {
            bc.wobble = 1;
            b = GobPool.Instantiate(bulletPrefab);
            bc = b.GetComponent<Bullet>();
            b.transform.SetPositionAndRotation(gunBarrel.position, gunBarrel.rotation * Quaternion.FromToRotation(Vector3.forward, new Vector3(0, 0.25f, 0.75f)));
            bc.Start();
            bc.Velocity = b.transform.forward * 30 + velocity;
            bc.wobble = 0.5f;

            b = GobPool.Instantiate(bulletPrefab);
            bc = b.GetComponent<Bullet>();
            b.transform.SetPositionAndRotation(gunBarrel.position, gunBarrel.rotation * Quaternion.FromToRotation(Vector3.forward, new Vector3(0, -0.25f, 0.75f)));
            bc.Start();
            bc.Velocity = b.transform.forward * 30 + velocity;
            bc.wobble = 0.5f;
        }
    }

    public float GetEnginePowerNormalized()
    {
        return effectiveEnginePower / maxEnginePower;
    }

    public float GetGunPowerNormalized()
    {
        return effectiveGunPower / maxGunPower;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("UpperBounds"))
        {
            inSpace = true;
            DialogueQueue.AddMessage("You're too close to the sun, Icarus!");
        }
        if (other.tag.Equals("DockingBay"))
        {
            DataDump.SetInt("PlayerDocked", 1);
            DockingSequence();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("UpperBounds"))
        {
            inSpace = false;
        }
    }

    public static ShipDriver Get()
    {
        return GameObject.FindWithTag("Player").GetComponent<ShipDriver>();
    }

    public bool IsDocked()
    {
        return docked;
    }

    public bool IsCranking()
    {
        return Time.time - lastCrankTime < 0.3f;
    }
}
