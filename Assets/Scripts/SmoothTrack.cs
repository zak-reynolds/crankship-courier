using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTrack : MonoBehaviour {

    public enum Mode { Horizontal, Vertical, Both, None }

    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 positionOffset;
    public Mode mode = Mode.Both;

    private Vector3 velocity = Vector3.zero;
	
	void Update ()
    {
        Vector3 effectiveTarget = transform.position;
        if (mode != Mode.None && target)
        {
            effectiveTarget = target.position + positionOffset;
            switch (mode)
            {
                case Mode.Horizontal:
                    effectiveTarget.y = transform.position.y;
                    break;
                case Mode.Vertical:
                    effectiveTarget.x = transform.position.x;
                    break;
            }
        }
        transform.position = Vector3.SmoothDamp(transform.position, effectiveTarget, ref velocity, smoothTime);
    }
}
