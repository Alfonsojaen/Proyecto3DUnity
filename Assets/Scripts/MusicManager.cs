using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    public AudioClip startMusic;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance.startMusic != null) {
            PlayStartMusic();
        }
    }

    public static void PlayBackgroundMusic() {
        Instance.audioSource.clip = Instance.backgroundMusic;
        Instance.audioSource.Play();
    }
    public static void PlayStartMusic() {
        Instance.audioSource.clip = Instance.startMusic;
        Instance.audioSource.Play();
    }
    public static void PauseMusic() {
        Instance.audioSource.Pause();
    }
    public static void StopMusic() {
        Instance.audioSource.Stop();
    }
    public static void ResetBackgroundMusic() {
        Instance.audioSource.Stop();
        Instance.audioSource.clip = Instance.backgroundMusic;
        PlayBackgroundMusic();
    }
}