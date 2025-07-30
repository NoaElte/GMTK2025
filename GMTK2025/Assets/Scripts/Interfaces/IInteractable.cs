using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    public string UseText { get; }

    public void Interact(Transform player);
}
