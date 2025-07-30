using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string baseInteractionText;

    public virtual string UseText => baseInteractionText;

    public UnityEvent onInteract;

    public virtual void Interact(Transform player)
    {
        onInteract.Invoke();
    }
}
