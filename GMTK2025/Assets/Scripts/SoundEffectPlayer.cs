using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField]
    private float volume = 1.0f;

    public void PlaySoundEffect()
    {
        audioSource.PlayOneShot(audioClip, volume);
    }
}
