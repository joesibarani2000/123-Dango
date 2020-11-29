using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public enum PlayType
{
    AUTO,
    OVERRIDE_PLAY,
    DEFAULT
}

public class AudioController : MonoBehaviour
{
    private static AudioController instance;
    private AudioLibrary audioLibrary;
    private AudioSource[] bgmSources;
    private Coroutine[] coroutines;
    private AudioSource sfxSource;
    private float timeFade = 1f;
    private int activeChannel;

    public static AudioController Instance {
        get
        {
            if (instance == null)
            {
                // Create a variable AudioController and call CreateInstance method directly from that class
                Debug.Log("Please call CreateInstance method before using this Controller");
            }

            return instance;
        }
    }

    public void CreateInstance()
    {
        createInstance();
    }

    public static void SetUpController([NotNull] AudioLibrary library, int bgmChannel = 1)
    {
        Instance.setUpController(library, bgmChannel);
    }

    public static void PlayBGM(string musicName, PlayType playType, bool loop = true)
    {
        Instance.playBGM(musicName, playType, loop);
    }

    public static void PlaySFX(string musicName)
    {
        Instance.playSFX(musicName);
    }

    private void createInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void playSFX(string musicName)
    {
        sfxSource.PlayOneShot(audioLibrary.GetSFXClips(musicName));
    }

    private void playBGM(string musicName, PlayType playType, bool loop)
    {
        switch (playType)
        {
            case PlayType.AUTO:
                checkBusyChannel();
                playBGMAuto(musicName, loop);
                break;
            case PlayType.OVERRIDE_PLAY:
                playBGMOverride(musicName, loop);
                break;
            case PlayType.DEFAULT:
                playBGMDefault(musicName, loop);
                break;
        }
    }

    private void playBGMAuto(string musicName, bool loop)
    {
        bool playFlag = false;

        for (int i = 0; i < bgmSources.Length; i++)
        {
            if (bgmSources[i].isPlaying && bgmSources[i].clip != null)
            {
                coroutines[0] = StartCoroutine(fadeAudio(bgmSources[i], false));
            }

            if (!bgmSources[i].isPlaying && bgmSources[i].clip == null && !playFlag)
            {
                playFlag = true;

                bgmSources[i].clip = audioLibrary.GetBGMClips(musicName);
                bgmSources[i].loop = loop;
                bgmSources[i].volume = 0;
                bgmSources[i].Play();

                activeChannel = i;

                coroutines[1] = StartCoroutine(fadeAudio(bgmSources[i], true));
            }
        }
    }

    private void playBGMDefault(string musicName, bool loop)
    {
        foreach (AudioSource source in bgmSources)
        {
            if (!source.isPlaying && source.clip == null)
            {
                source.clip = audioLibrary.GetBGMClips(musicName);
                source.loop = loop;
                source.volume = 1;
                source.Play();

                return;
            }
        }
    }

    private void playBGMOverride(string musicName, bool loop)
    {
        foreach (AudioSource source in bgmSources)
        {
            if (!source.isPlaying && source.clip == null)
            {
                source.clip = audioLibrary.GetBGMClips(musicName);
                source.loop = loop;
                source.volume = 0;
                source.Play();

                StartCoroutine(fadeAudio(source, true));

                return;
            }
        }
    }

    private void checkBusyChannel()
    {
        if (!availableChannel())
        {
            forceStopBGM();
        }
    }

    private bool availableChannel()
    {
        foreach (AudioSource source in bgmSources)
        {
            if (!source.isPlaying) return true;
        }

        return false;
    }

    private void forceStopBGM()
    {
        activeChannel--;

        if (activeChannel < 0) activeChannel = bgmSources.Length - 1;

        StopCoroutine(coroutines[activeChannel]);
        bgmSources[activeChannel].volume = 0;
        bgmSources[activeChannel].clip = null;
        bgmSources[activeChannel].Stop();

    }

    private void setUpController(AudioLibrary library, int bgmChannel)
    {
        instance.bgmSources = new AudioSource[bgmChannel];
        instance.coroutines = new Coroutine[bgmChannel];

        for (int i = 0; i < bgmChannel; i++)
        {
            instance.bgmSources[i] = instance.gameObject.AddComponent<AudioSource>();
            instance.bgmSources[i].playOnAwake = false;
            instance.bgmSources[i].loop = false;
        }

        instance.sfxSource = instance.gameObject.AddComponent<AudioSource>();
        instance.sfxSource.playOnAwake = false;
        instance.sfxSource.loop = false;

        instance.audioLibrary = library;
    }

    private IEnumerator fadeAudio(AudioSource source, bool fadeIn)
    {
        float timeElapse = timeFade;

        while (timeElapse > 0)
        {
            source.volume = fadeIn ? (1f - timeElapse/timeFade) : (timeElapse / timeFade);
            yield return null;
            timeElapse -= Time.deltaTime;
        }

        source.volume = fadeIn ? 1f : 0f;

        if (!fadeIn)
        {
            source.Stop();
            source.clip = null;
        }
    }

}
