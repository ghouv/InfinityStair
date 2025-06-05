using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip dieClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBGM()
    {
        audioSource.clip = bgmClip;
        audioSource.loop = true;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public void PlayDieSound()
    {
        audioSource.Stop();
        audioSource.clip = dieClip;
        audioSource.loop = false;
        audioSource.volume = 1f;
        audioSource.Play();
    }
}