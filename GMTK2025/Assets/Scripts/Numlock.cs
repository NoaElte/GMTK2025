using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Numlock : MonoBehaviour
{
    public List<int> Code = new List<int>();

    public UnityEvent OnCorrectEnter;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip correctAudioClip;
    [SerializeField]
    private AudioClip incorrectAudioClip;
    [SerializeField]
    private float volume = 1.0f;

    private List<int> nums;

    private void Start()
    {
        nums = new List<int>();
    }

    public void AddNum(int num)
    {
        nums.Add(num);
    }

    public void BackSpace()
    {
        nums.RemoveAt(nums.Count - 1);
    }

    public void Enter()
    {
        if (CheckCode())
        {
            OnCorrectEnter.Invoke();
            audioSource.PlayOneShot(correctAudioClip, volume);
        }
        else
        {
            nums.Clear();
            audioSource.PlayOneShot(incorrectAudioClip, volume);
        }
    }

    private bool CheckCode()
    {
        if (nums.Count != Code.Count)
            return false;

        for (int i = 0; i < nums.Count; i++)
        {
            if (nums[i] != Code[i])
                return false;
        }

        return true;
    }

    private void UpdateVisuals()
    {

    }
}
