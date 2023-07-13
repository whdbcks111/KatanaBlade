using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class ScreenEffectManager : MonoBehaviour
{
    public static ScreenEffectManager Instance { get; private set; }
    private Volume _volume;

    private float _initHueShift, _initContrast, _initSaturation, 
        _initBloomIntensity, _initBloomThreshold;

    private void Awake()
    {
        Instance = this;
        _volume = GetComponent<Volume>();

        if (_volume.profile.TryGet(out ColorAdjustments ca))
        {
            _initHueShift = ca.hueShift.value;
            _initContrast = ca.contrast.value;
            _initSaturation = ca.saturation.value;
        }
        if (_volume.profile.TryGet(out Bloom bloom))
        {
            _initBloomIntensity = bloom.intensity.value;
            _initBloomThreshold = bloom.threshold.value;
        }
    }

    public void ResetAll()
    {
        ResetBloomIntensity();
        ResetBloomThreshold();
        ResetContrast();
        ResetSaturation();
        ResetHueShift();
    }

    public void SetHueShift(float shift)
    {
        if (_volume.profile.TryGet(out ColorAdjustments ca))
        {
            ca.hueShift.Override(shift);
        }
    }

    public void ResetHueShift()
    {
        SetHueShift(_initHueShift);
    }

    public void SetContrast(float value)
    {
        if (_volume.profile.TryGet(out ColorAdjustments ca))
        {
            ca.contrast.Override(value);
        }
    }

    public void ResetContrast()
    {
        SetContrast(_initContrast);
    }

    public void SetSaturation(float sat)
    {
        if (_volume.profile.TryGet(out ColorAdjustments ca))
        {
            ca.saturation.Override(sat);
        }
    }

    public void ResetSaturation()
    {
        SetSaturation(_initSaturation);
    }

    public void SetBloomIntensity(float intensity)
    {
        if (_volume.profile.TryGet(out Bloom bloom))
        {
            bloom.intensity.Override(intensity);
        }
    }

    public void ResetBloomIntensity()
    {
        SetBloomIntensity(_initBloomIntensity);
    }

    public void SetBloomThreshold(float threshold)
    {
        if (_volume.profile.TryGet(out Bloom bloom))
        {
            bloom.threshold.Override(threshold);
        }
    }

    public void ResetBloomThreshold()
    {
        SetBloomThreshold(_initBloomThreshold);
    }
}
