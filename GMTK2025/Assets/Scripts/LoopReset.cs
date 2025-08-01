using UnityEngine;
using UnityEngine.Events;

public class LoopReset : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnReset;

    private void OnEnable()
    {
        GameManager.OnReset += Reset;
    }

    private void OnDisable()
    {
        GameManager.OnReset -= Reset;
    }

    private void Reset()
    {
        OnReset?.Invoke();
    }
}
