using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public class ImageFiller : MonoBehaviour {

    public enum FillTarget { Engine, Gun, HP };
    public FillTarget target;

    private Image filledImage;

    private void Start()
    {
        filledImage = GetComponent<Image>();
        switch (target) {
            case FillTarget.Engine:
                GameObject.FindWithTag("Player").GetComponent<ShipDriver>().EnginePowerChanged.AddListener(UpdateFill);
                break;
            case FillTarget.Gun:
                GameObject.FindWithTag("Player").GetComponent<ShipDriver>().GunPowerChanged.AddListener(UpdateFill);
                break;
            case FillTarget.HP:
                GameObject.FindWithTag("Player").GetComponent<ShipDriver>().HPChanged.AddListener(UpdateFill);
                break;
        }
    }
    void UpdateFill (float amount) {
        filledImage.fillAmount = amount;
	}
}
