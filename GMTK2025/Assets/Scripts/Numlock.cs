using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Numlock : MonoBehaviour
{
    public List<int> Code = new List<int>();

    public UnityEvent OnCorrectEnter;

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
            OnCorrectEnter.Invoke();
        else
        {
            nums.Clear();
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
