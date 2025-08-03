using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    public SphereCollider radioSphereCollider;
    public float loweredVolume = 0.2f;
    public float normalVolume = 1.0f;
    public float transitionTime = 1.0f;

    private AudioSource audioSource;
    private Coroutine volumeCoroutine;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == radioSphereCollider)
        {
            StartVolumeTransition(loweredVolume);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == radioSphereCollider)
        {
            StartVolumeTransition(normalVolume);
        }
    }

    private void StartVolumeTransition(float targetVolume)
    {
        if (volumeCoroutine != null)
            StopCoroutine(volumeCoroutine);
        volumeCoroutine = StartCoroutine(VolumeTransition(targetVolume));
    }

    private IEnumerator VolumeTransition(float targetVolume)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;
        while (elapsed < transitionTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / transitionTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = targetVolume;
    }
}
