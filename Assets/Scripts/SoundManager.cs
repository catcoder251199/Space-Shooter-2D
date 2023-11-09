using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _effectSource;
    [SerializeField] private AudioClip _bossMusic;
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField, Range(0, 1)] private float _effectVolume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusicClip(AudioClip clip, float volume)
    {
        _musicSource.clip = clip;
        _musicSource.volume = volume;
        _musicSource.Play();
    }

    public void PlayBossBackgroundMusic()
    {
        PlayMusicClip(_bossMusic, _musicSource.volume);
    }
    public void PlayBackgroundMusic()
    {
        PlayMusicClip(_bossMusic, _musicSource.volume);
    }

    public void PlayEffectOneShot(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip, _effectVolume);
    }

    public void PlayEffectAtPoint(AudioClip clip, Vector3 pos)
    {
        _effectSource.PlayOneShot(clip, _effectVolume);
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

}
