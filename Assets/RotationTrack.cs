using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTrack : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.5f;

    private Vector3 velocity = Vector3.zero;
    
    void Update ()
    {
        Quaternion effectiveTarget = transform.rotation;
        if (target)
        {
            effectiveTarget = target.rotation;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, effectiveTarget, smoothTime);
    }
}
