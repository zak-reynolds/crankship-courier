using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightSpinShipConnector : MonoBehaviour {

    private StraightSpin ss;
	void Start () {
        ss = GetComponent<StraightSpin>();
        ShipDriver.Get().EnginePowerChanged.AddListener(OnEngineChanged);
	}
	
	void OnEngineChanged(float value) {
		ss.speed = 180 + (value * 720);
	}
}
