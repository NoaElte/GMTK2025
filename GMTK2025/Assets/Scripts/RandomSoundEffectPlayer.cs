using System.Collections.Generic;
using UnityEngine;

public class RandomSoundEffectPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private List<AudioClip> audioClips;
    [SerializeField]
    private float volume = 1.0f;

    public void PlaySoundEffect()
    {
        if (audioClips == null || audioClips.Count == 0 || audioSource == null)
            return;

        // Pick a random footstep sound
        int index = Random.Range(0, audioClips.Count);
        AudioClip clip = audioClips[index];
        Debug.Log(clip.name);

        audioSource.PlayOneShot(clip, volume);
    }
}
