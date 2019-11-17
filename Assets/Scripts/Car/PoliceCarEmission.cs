using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class PoliceCarEmission : MonoBehaviour
{
    public Light2D RedLight;
    public Light2D BlueLight;
    private int _State = 0;
    float _IntensityValue = 0.1f;

    void FixedUpdate()
    {
        if (_State == 0)
        {
            RedLight.intensity -= _IntensityValue;
            if (RedLight.intensity <= 0)
                _State = 1;
        }

        if (_State == 1)
        {
            BlueLight.intensity += _IntensityValue;
            if (BlueLight.intensity >= 1)
                _State = 2;
        }

        if (_State==2)
        {
            BlueLight.intensity -= _IntensityValue;
            if (BlueLight.intensity <= 0)
                _State = 3;
        }

        if(_State==3)
        {
            RedLight.intensity += _IntensityValue;
            if (RedLight.intensity >= 1)
                _State = 0;
        }
    }
}
