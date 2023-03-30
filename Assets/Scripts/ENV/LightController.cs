using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightController : MonoBehaviour
{

    public Light spotLight;

    bool _isLightActive;
    float _defaultIntensity;

    private void Start()
    {
        _defaultIntensity = spotLight.intensity;
        _isLightActive = spotLight.isActiveAndEnabled;
        Debug.Log(_isLightActive);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        if (_isLightActive)
        {
            spotLight.DOIntensity(0, .25f).OnComplete(() => spotLight.gameObject.SetActive(false));
            _isLightActive = false;
        }
        else
        {
            spotLight.gameObject.SetActive(true);
            spotLight.DOIntensity(_defaultIntensity, .25f);
            _isLightActive = true;
        }
    }

}
