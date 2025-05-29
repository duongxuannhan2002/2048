using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public static bool isMusicOn;
    public static bool isSfxOn;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip bgmMusic;
    public AudioClip mergeSound;
    public AudioClip moveSound;

    public AudioClip winSound;
    public AudioClip loseSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
            isSfxOn = PlayerPrefs.GetInt("SfxOn", 1) == 1;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        PlayMusic();
    }

    public static void ToggleMusic()
    {
        if (Instance == null) return;

        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt("SfxOn", isSfxOn ? 1 : 0);
        PlayerPrefs.Save();
        if (isMusicOn)
            Instance.PlayMusic();
        else
            Instance.StopMusic();
    }

    public static void ToggleSFX()
    {
        isSfxOn = !isSfxOn;
        PlayerPrefs.SetInt("MusicOn", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void PlayMusic()
    {
        if (!isMusicOn) return;

        musicSource.clip = bgmMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayMoveSound()
    {
        if (isSfxOn)
            sfxSource.PlayOneShot(moveSound);
    }

    public void PlayMergeSound()
    {
        if (isSfxOn)
            sfxSource.PlayOneShot(mergeSound);
    }

    public void PlayWinSound()
    {
        if (isSfxOn)
            sfxSource.PlayOneShot(winSound);
    }

    public void PlayLoseSound()
    {
        if (isSfxOn)
            sfxSource.PlayOneShot(loseSound);
    }
}
