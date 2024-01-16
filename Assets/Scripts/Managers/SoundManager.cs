using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Bgm,
    Sfx,
    Max
}

public class SoundManager : Singleton<SoundManager>
{
    private class AudioInfo
    {
        public AudioSource audioSource;
        public float audioVolume;
    }

    private readonly string path = "Sounds/";
    private readonly Dictionary<string, AudioClip> audioClips = new();

    private readonly Dictionary<SoundType, AudioInfo> audioInfos = new();

    protected override bool IsDontDestroying => true;
    private const string AUDIO_SOURCE_NAME_BY_PITCH = "Pitch";
    private Coroutine bgmFadeCoroutine;

    protected override void OnCreated()
    {
        var clips = Resources.LoadAll<AudioClip>(path);
        foreach (var clip in clips)
            audioClips.Add(clip.name, clip);

        for (var soundType = SoundType.Bgm; soundType < SoundType.Max; soundType++)
            AddAudioInfo(soundType);

        PoolManager.Instance.JoinPoolingData(AUDIO_SOURCE_NAME_BY_PITCH, audioInfos[SoundType.Sfx].audioSource.gameObject);
    }

    public void UpdateVolume(SoundType soundType, float sound)
    {
        audioInfos[soundType].audioVolume = sound;
        audioInfos[soundType].audioSource.volume = sound;
    }

    private void AddAudioInfo(SoundType soundType)
    {
        var audioSourceObj = new GameObject(soundType.ToString());
        audioSourceObj.transform.SetParent(transform);

        var audioInfo = new AudioInfo
        {
            audioSource = audioSourceObj.AddComponent<AudioSource>(),
            audioVolume = 1
        };
        if (soundType == SoundType.Bgm)
            audioInfo.audioSource.loop = true;
        audioInfos.Add(soundType, audioInfo);
    }

    public AudioSource GetAudioSource(SoundType soundType = SoundType.Sfx)
    {
        return audioInfos[soundType].audioSource;
    }

    private IEnumerator PlayFadeBgm(string soundName, float multipleVolume = 1, float pitch = 1)
    {
        var audioInfo = audioInfos[SoundType.Bgm];
        var audioSource = audioInfo.audioSource;

        float volume = audioInfo.audioVolume * multipleVolume;
        if (audioSource.isPlaying)
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= Time.deltaTime * volume;
                yield return null;
            }

            if (string.IsNullOrEmpty(soundName))
            {
                audioSource.Stop();
                yield break;
            }
        }

        if (!audioClips.ContainsKey(soundName))
        {
            Debug.Log("그 소리 없음!"); 
            yield break;
        }

        var clip = audioClips[soundName];

        audioSource.clip = clip;
        audioSource.volume = 0;
        audioSource.time = 0;
        audioSource.pitch = pitch;
        audioSource.Play();

        while (audioSource.volume < volume)
        {
            audioSource.volume += Time.deltaTime * volume;
            yield return null;
        }

        audioSource.volume = volume;
    }

    public void PlaySound(string soundName, SoundType soundType = SoundType.Sfx, float multipleVolume = 1, float pitch = 1)
    {
        if (soundType == SoundType.Bgm)
        {
            if (bgmFadeCoroutine != null)
                StopCoroutine(bgmFadeCoroutine);
            bgmFadeCoroutine = StartCoroutine(PlayFadeBgm(soundName, multipleVolume, pitch));
            return;
        }

        if (string.IsNullOrEmpty(soundName))
        {
            audioInfos[soundType].audioSource.Stop();
            return;
        }

        if (!audioClips.ContainsKey(soundName))
        {
            Debug.Log("그 소리 없음!");
            return;
        }

        var clip = audioClips[soundName];
        var audioInfo = audioInfos[soundType];
        var audioSource = audioInfo.audioSource;

        if (Math.Abs(pitch - audioSource.pitch) > 0.03f)
        {
            audioSource = PoolManager.Instance.Init(AUDIO_SOURCE_NAME_BY_PITCH, transform).GetComponent<AudioSource>();
            audioSource.pitch = pitch;
        }

        audioSource.PlayOneShot(clip, audioInfo.audioVolume * multipleVolume);
    }
}