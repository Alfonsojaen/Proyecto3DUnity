using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;
    private static AudioSource audioSource;
    public AudioClip gemCollectedClip;
    public AudioClip jumpClip;
    public AudioClip gameLostClip;
    public AudioClip gameWinClip;

    private void Awake() {
        // Configuración Singleton
        if (Instance == null) {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            
            // Evitar que el objeto se destruya al cambiar de escena
            DontDestroyOnLoad(gameObject);

            // Configuración 3D para el AudioSource
            audioSource.spatialBlend = 1.0f; // 0 = 2D, 1 = 3D
            audioSource.rolloffMode = AudioRolloffMode.Linear; // Atenuación lineal
            audioSource.minDistance = 1.0f; // Distancia mínima a la que se escucha sin cambios
            audioSource.maxDistance = 50.0f; // Distancia máxima antes de dejar de oírse
            
        } else {
            Destroy(gameObject); // Destruir si ya hay una instancia existente
        }
    }

    // Métodos de reproducción de sonido
    public static void playGemCollectedSound() {
        audioSource.PlayOneShot(Instance.gemCollectedClip);
    }

    public static void playJumpSound() {
        audioSource.PlayOneShot(Instance.jumpClip);
    }

    public static void playGameLostSound() {
        audioSource.PlayOneShot(Instance.gameLostClip);
    }

    public static void playGameWinSound() {
        audioSource.PlayOneShot(Instance.gameWinClip);
    }
}