using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;

public class SoundSource : SerializedMonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    [Header("Ambient Sound")]
    private Dictionary<string,AudioClip> _audioClip;

    [SerializeField]
    [Header("Sound Audio Source")]
    private Dictionary<string, AudioSource> _audioSources;

    private void Reset()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        MessageBroker.Default.Receive<PlaySoundEvent>().Subscribe(playSoundEvent => { PlayAudioSource(playSoundEvent.NameSound); });
        MessageBroker.Default.Receive<PlaySoundEventWithTimeLife>().Subscribe(playSoundEvent => { PlayAudioSourceLifeTime(playSoundEvent.NameSound, playSoundEvent.TimeLife); });
        MessageBroker.Default.Receive<StopSoundEvent>().Subscribe(stopSoundEvent => { StopAudioSource(stopSoundEvent.NameSound); });
    }

    public void SetAndPlayAmbientSound()
    {
        _audioSource.clip = _audioClip["Chapter" + LevelManager.Instance.CurrentChapter];
        _audioSource.Play();
    }
    public void StopAmbientSound()
    {
        _audioSource.Stop();
    }

    public void PlayAudioSource(string keySourceAudio)
    {
        _audioSources[keySourceAudio].Play();
    }
    public void PlayAudioSourceLifeTime(string keySourceAudio, float timeLife)
    {
        _audioSources[keySourceAudio].loop = true;
        _audioSources[keySourceAudio].Play();
        StartCoroutine(EnableLoop(_audioSources[keySourceAudio], false, timeLife));
    }
    IEnumerator EnableLoop(AudioSource audioSource, bool enable, float timeWait)
    {
        yield return new WaitForSeconds(timeWait);
        audioSource.loop = enable;
    }
    public void StopAudioSource(string keySourceAudio)
    {
        _audioSources[keySourceAudio].Stop();
    }
}
