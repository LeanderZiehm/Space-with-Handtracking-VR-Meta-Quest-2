using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private int _index = -1;
    private AudioSource _audioSource;
    public static MusicManager _instance;
    private AudioClip[] _audioClips;
    void Awake()
    {
        _instance = this;
        _audioSource = GetComponent<AudioSource>();
    }
    public void SetAudioClip(AudioClip[] audioClips)
    {
        _audioClips = audioClips;
        if (!_audioSource.isPlaying)
        {
            PlayRandomSong();
        }
    }
    public void PlayRandomSong()
    {
        _index = Random.Range(0, _audioClips.Length);
        PlaySong(_index);
     
    }
    public void PlayNextSong()
    {
        CancelInvoke("PlayNextSong");
        _index++;
        if (_index >= _audioClips.Length)
        {
            _index = 0;
        }

        PlaySong(_index);
    }

    public void PlayPrevSong()
    {
        _index--;
        if (_index < 0)
        {
            _index = _audioClips.Length - 1;
        }
        PlaySong(_index);
    }

    private void PlaySong(int index)
    {
        RandomizeSound();
        _audioSource.clip = _audioClips[index];
        _audioSource.Play();
        CancelInvoke("PlayNextSong");
        Invoke("PlayNextSong", _audioClips[index].length);// recursive loop
    }
    private void RandomizeSound()
    {
        _audioSource.volume = Random.Range(0.8f, 1f);
        _audioSource.pitch = Random.Range(0.8f, 1.2f);
    }

    public void SetVolume(float vol)
    {
        _audioSource.volume = vol;
    }
    
    public bool SwapAndGetIsMuted()
    {
        _audioSource.mute = !_audioSource.mute;
        return _audioSource.mute; 
    }
    public bool SwapAndGetIsPaused()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
        }
        else
        {
            _audioSource.Play();
        }
        return !_audioSource.isPlaying; 
    }

    public float GetVolume()
    {
        return _audioSource.volume;
    }


    public void PlayClip(AudioClip clip)
    {
        RandomizeSound();
        _audioSource.clip = clip;
        _audioSource.Play();
        CancelInvoke("PlayNextSong");
        Invoke("PlayNextSong", clip.length);// recursive loop
    }
}
