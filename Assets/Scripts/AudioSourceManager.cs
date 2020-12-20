using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    [Header("UI")]
    public AudioClip _click;

    [Header("Game")]
    public AudioClip SnowBallImpact;
    public AudioClip SnowBallThrow;
    public AudioClip SnowManStep;
    public AudioClip WinAudioClip;
    public AudioClip LooseAudioClip;

    #region Singleton

    private static AudioSourceManager _instance;
    public static AudioSourceManager Instance => _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(this);
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    public void PlayClick()
    {
    }

    public void PlayWin()
    {
        _audioSource.clip = WinAudioClip;
        _audioSource.Play();
    }

    public void PlayLose()
    {
        _audioSource.clip = LooseAudioClip;
        _audioSource.Play();
    }

    public AudioSource CreateAudioSource(Transform parent, AudioClip clip)
    {
        AudioSource source = (new GameObject("as")).AddComponent<AudioSource>();
        source.transform.SetParent(parent);
        source.playOnAwake = false;
        source.clip = clip;
        parent.localPosition = Vector3.zero;
        return source;
    }
}
