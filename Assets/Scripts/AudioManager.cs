using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource channelBGMusic;
    public AudioLowPassFilter lowPassFilter;
    public AudioSource[] channels;

    public AudioClip BGMusic;
    public AudioClip[] gameSounds;

    [Header("BG Music Variables")]
    [SerializeField] float fadeInTime;
    [SerializeField] float fadeOutTime;
    [SerializeField] float lowPassHigh;
    [SerializeField] float lowPassLow;

    [Header("Menu Music Variables")]
    [SerializeField] float loopStart;
    [SerializeField] float loopEnd;
    [SerializeField] bool stopLoop;

    private static AudioManager instance;
    private float effectsVolume;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        effectsVolume = 1.0f;
        stopLoop = false;
        lowPassFilter.enabled = false;

        //check for BG music and play if not playing
        if (channelBGMusic != null && BGMusic != null)
        {
            channelBGMusic.clip = BGMusic;
            channelBGMusic.loop = true;
            channelBGMusic.Play();
        }
    }

    private void Update()
    {
        if (!stopLoop)
        {
            if (channelBGMusic.isPlaying && channelBGMusic.time >= loopEnd)
            {
                channelBGMusic.time = loopStart;
            }
        }
    }

    //check for an available channel and load clip into that channel. Then play sound. 8 channels available for multiple amounts of sounds.
    public void PlaySound(string soundType)
    {
        AudioSource freeChannel = null;

        for (int i = 0; i < channels.Length; i++)
        {
            if (!channels[i].isPlaying)
            {
                freeChannel = channels[i];
                freeChannel.pitch = 1f;
                freeChannel.volume = effectsVolume;
                break;
            }
        }

        switch (soundType)
        {
            case "blockDrop":
                freeChannel.clip = gameSounds[0];
                freeChannel.pitch = Random.Range(0.7f, 2f);
                freeChannel.Play();
                break;
            case "rotation":
                freeChannel.clip = gameSounds[1];
                freeChannel.pitch = 3f;
                freeChannel.volume = 0.3f * effectsVolume;
                freeChannel.Play();
                break;
            case "powerupReady":
                freeChannel.clip = gameSounds[2];
                freeChannel.Play();
                break;
            case "usePowerup":
                freeChannel.clip = gameSounds[3];
                freeChannel.pitch = 0.7f;
                freeChannel.volume = 1.1f * effectsVolume;
                freeChannel.Play();
                break;
            default:
                break;
        }
    }

    public IEnumerator SlowBGMusic(float pitch)
    {
        lowPassFilter.cutoffFrequency = lowPassHigh;
        lowPassFilter.enabled = true;
        float elapsed = 0f;

        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeInTime;
            channelBGMusic.pitch = Mathf.Lerp(1f, pitch, t);
            lowPassFilter.cutoffFrequency = Mathf.Lerp(lowPassHigh, lowPassLow, t);
            yield return null;
        }
        channelBGMusic.pitch = pitch;
    }

    public void BGMusicLowPass()
    {
        lowPassFilter.cutoffFrequency = lowPassLow;
        lowPassFilter.enabled = true;
    }

    public IEnumerator NormalBGMusic(float pitch)
    {
        float elapsed = 0f;

        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutTime;
            channelBGMusic.pitch = Mathf.Lerp(pitch, 1f, t);
            lowPassFilter.cutoffFrequency = Mathf.Lerp(lowPassLow, lowPassHigh, t);
            yield return null;
        }
        channelBGMusic.pitch = 1f;
        lowPassFilter.enabled = false;
    }

    public void AdjustFolder()
    {
        transform.SetParent(GameObject.Find("Managers").transform);
    }

    public void StopLoop() 
    {
        stopLoop = true;
    }

    public void AdjustMusicVolume() 
    {
        channelBGMusic.volume = GameObject.Find("MusicSlider").GetComponent<Slider>().value;
    }

    public void AdjustEffectsVolume() 
    {
        effectsVolume = GameObject.Find("EffectsSlider").GetComponent<Slider>().value;
    }
}
