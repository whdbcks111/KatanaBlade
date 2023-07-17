using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public float SFXVolumeMultiplier = 1f;
    public float BGMVolumeMultiplier = 1f;

    [SerializeField] private AudioClip[] _bgmClips;
    [SerializeField] private AudioInfo[] _sfxClips;

    private AudioSource _sourcePrefab, _bgmSource;

    private readonly Dictionary<string, AudioClip> _sfxClipMap = new();
    private readonly Queue<AudioSource> _waitingSources = new();

    private readonly Queue<AudioClip> _bgmQueue = new();

    private void Awake()
    {
        Instance = this;
        
        _sourcePrefab = new GameObject("Sound").AddComponent<AudioSource>();
        _bgmSource = Instantiate(_sourcePrefab, transform);
        _bgmSource.name = "BGM";

        foreach (AudioInfo info in _sfxClips)
        {
            _sfxClipMap.Add(info.name, info.clip);
        }
        foreach(AudioClip bgmClip in _bgmClips)
        {
            _bgmQueue.Enqueue(bgmClip);
        }
    }

    private void Update()
    {
        if (_bgmQueue.Count == 0) return;

        if (_bgmSource.clip == null)
        {
            _bgmSource.clip = _bgmQueue.Peek();
        }

        if(_bgmSource.time >= _bgmQueue.Peek().length)
        {
            _bgmQueue.Enqueue(_bgmQueue.Dequeue());
            _bgmSource.clip = _bgmQueue.Peek();
        }

        if(!_bgmSource.isPlaying)
            _bgmSource.Play();

        _bgmSource.volume = BGMVolumeMultiplier;
    }

    public void PlaySFX(string name, float volume = 1f, float pitch = 1f)
    {
        volume *= SFXVolumeMultiplier;
        var clip = _sfxClipMap[name];
        if (clip == null) return;
        var source = _waitingSources.TryDequeue(out AudioSource result) ? result : Instantiate(_sourcePrefab, transform);
        source.gameObject.SetActive(true);
        source.pitch = pitch;
        source.PlayOneShot(clip, volume);
        StartCoroutine(PlayRoutine(clip.length / pitch, source));
    }

    private IEnumerator PlayRoutine(float length, AudioSource source)
    {
        yield return new WaitForSeconds(length);
        source.gameObject.SetActive(false);
        _waitingSources.Enqueue(source);
    }
}

[Serializable]
public struct AudioInfo
{
    [SerializeField] public string name;
    [SerializeField] public AudioClip clip;
}