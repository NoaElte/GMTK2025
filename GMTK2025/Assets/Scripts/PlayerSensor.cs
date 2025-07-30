using UnityEngine;
using UnityEngine.Events;

public class PlayerSensor : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        onEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        onExit.Invoke();
    }
}
